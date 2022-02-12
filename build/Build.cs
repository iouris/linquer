using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.CoverallsNet;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[GitHubActions(
    "ci",
    GitHubActionsImage.UbuntuLatest,
    //auto-generate /ci/ci.yml
    AutoGenerate = false,
    OnPushBranches = new[] { "main" },
    OnPullRequestBranches = new[] { "main" },

    InvokedTargets = new[] { nameof(PublishCoverageReport) },
    //will generate CI env command to set corresponding env vars to values imported from secrets, ie
    //TOKEN_1 = ${{ secrets.TOKEN_1 }}
    ImportSecrets = new[] { "GITHUB_TOKEN" }
)]
public class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(build => build.RunUnitTests);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    private readonly Solution Solution = null!;

    //[GitRepository]
    //private readonly GitRepository GitRepository = null!;
    //[GitVersion] readonly GitVersion GitVersion;

    //RootDirectory is the repository folder ie ..\repos\linquer
    private static readonly AbsolutePath SourceDirectory = RootDirectory / "src";
    private static readonly AbsolutePath TestsDirectory = RootDirectory / "tests";
    private static readonly AbsolutePath ArtifactsDirectory = RootDirectory / "artifacts";
    private static readonly AbsolutePath LinquerTestsProjectDirectory = TestsDirectory / "Linquer";
    private const string TestResultsDirectoryName = "TestResults";
    private static readonly AbsolutePath LinquerTestResultsDirectory = LinquerTestsProjectDirectory / TestResultsDirectoryName;

    public Target Clean => definition =>
        definition
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    public Target Restore => definition =>
        definition
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s =>
                s
                .SetProjectFile(Solution)
            );
        });

    public Target Compile => definition =>
        definition
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s =>
                s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                //.SetFileVersion(GitVersion.AssemblySemFileVer)
                //.SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore()
            );
        });

    public Target RunUnitTests => definition =>
        definition
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s =>
                s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetVerbosity(DotNetVerbosity.Normal)

                //unit tests code coverage
                .EnableCollectCoverage()
                .SetCoverletOutput(LinquerTestResultsDirectory + "/")
                .SetCoverletOutputFormat(CoverletOutputFormat.lcov)
            ); ;
        });

    public Target PublishCoverageReport => definition =>
        definition
        .DependsOn(RunUnitTests)
        .Executes(() =>
        {
            var coverallsNetSettings =
                new LatestCoverallsNetSettings { LCov = true }
                .SetInput(LinquerTestResultsDirectory / "coverage.info")
                .SetRepoTokenVariable("GITHUB_TOKEN");

            var output = CoverallsNetTasks.CoverallsNet(coverallsNetSettings);
            return output;
        });
}
