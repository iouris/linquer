using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Linquer.Tests;
public static class TestHelpers
{
    [return: NotNullIfNotNull("value")]
    public static Expression<Func<int, bool>>? AsResultOfLambdaWith1Argument(this bool? value) =>
        value == null ? null : _ => value.Value;

    [return: NotNullIfNotNull("value")]
    public static Expression<Func<int, int, bool>>? AsResultOfLambdaWith2Arguments(this bool? value) =>
        value == null ? null : (_, _) => value.Value;

    [return: NotNullIfNotNull("value")]
    public static Expression<Func<int, int, int, bool>>? AsResultOfLambdaWith3Arguments(this bool? value) =>
        value == null ? null : (_, _, _) => value.Value;
}
