using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace Linquer.Tests;

public class LambdaOperatorsTests
{
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]

    [InlineData(false, null, false)]
    [InlineData(true, null, true)]
    [InlineData(null, false, false)]
    [InlineData(null, true, true)]

    [InlineData(null, null, null)]
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
    [InlineData(false, false, false)]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]

    [InlineData(false, null, false)]
    [InlineData(true, null, true)]
    [InlineData(null, false, false)]
    [InlineData(null, true, true)]

    [InlineData(null, null, null)]
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
}
