using Nuke.Common.Tooling;
using Nuke.Common.Tools.CoverallsNet;
using System;

[Serializable]
public class LatestCoverallsNetSettings : CoverallsNetSettings
{
    public bool LCov { get; set; }

    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        var args =
            base.ConfigureProcessArguments(arguments)
            .Add("--lcov", LCov);
        return args;
    }
}