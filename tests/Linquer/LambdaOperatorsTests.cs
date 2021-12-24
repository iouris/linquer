using FluentAssertions;
using Xunit;

namespace Linquer.Tests;

public class LambdaOperatorsTests
{
    [Theory]
    [MemberData(nameof(AndOperatorArgumentsAndExpectedResults))]
    public void And_should_honour_nulls_and_return_expected_results(bool? leftValue, bool? rightValue, bool? expectedResult)
    {
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith1Argument().And(rightValue.AsResultOfLambdaWith1Argument());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default);
            andResult.Should().Be(expectedResult);
        }
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith2Arguments().And(rightValue.AsResultOfLambdaWith2Arguments());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default, default);
            andResult.Should().Be(expectedResult);
        }
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith3Arguments().And(rightValue.AsResultOfLambdaWith3Arguments());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default, default, default);
            andResult.Should().Be(expectedResult);
        }
    }

    [Theory]
    [MemberData(nameof(OrOperatorArgumentsAndExpectedResults))]
    public void Or_should_honour_nulls_and_return_expected_results(bool? leftValue, bool? rightValue, bool? expectedResult)
    {
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith1Argument().Or(rightValue.AsResultOfLambdaWith1Argument());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default);
            andResult.Should().Be(expectedResult);
        }
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith2Arguments().Or(rightValue.AsResultOfLambdaWith2Arguments());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default, default);
            andResult.Should().Be(expectedResult);
        }
        {
            var combinedLambda = leftValue.AsResultOfLambdaWith3Arguments().Or(rightValue.AsResultOfLambdaWith3Arguments());
            var inlinedLambda = combinedLambda?.Inline();
            var andResult = inlinedLambda?.Compile().Invoke(default, default, default);
            andResult.Should().Be(expectedResult);
        }
    }

    public static IEnumerable<object[]> AndOperatorArgumentsAndExpectedResults() =>
        BoolOperatorArgumentsAndExpectedResults((left, right) => left && right);

    public static IEnumerable<object[]> OrOperatorArgumentsAndExpectedResults() =>
        BoolOperatorArgumentsAndExpectedResults((left, right) => left || right);

    private static readonly bool?[] NullableBoolPermutations = new bool?[] { null, false, true };

    private static IEnumerable<object[]> BoolOperatorArgumentsAndExpectedResults(Func<bool, bool, bool> op) =>
        from left in NullableBoolPermutations
        from right in NullableBoolPermutations
        select new object[] { left, right, left == null ? right : right == null ? left : op(left.Value, right.Value) };
}
