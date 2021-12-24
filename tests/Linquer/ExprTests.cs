using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace Linquer.Tests;

public class ExprTests
{
    [Fact]
    public void CreateShouldReturnTheGivenExpression()
    {
        Expression<Func<int, int>> expr = a => a;
        Expr.Create(expr).Should().BeSameAs(expr);
    }
}
