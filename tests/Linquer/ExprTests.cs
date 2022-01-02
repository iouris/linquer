using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace Linquer.Tests;

public class ExprTests
{
    [Fact]
    public void Create0ShouldReturnTheGivenExpression()
    {
        Expression<Func<int>> expr = () => 0;
        Expr.Create(expr).Should().BeSameAs(expr);
    }

    [Fact]
    public void Create1ShouldReturnTheGivenExpression()
    {
        Expression<Func<int, int>> expr = a => a;
        Expr.Create(expr).Should().BeSameAs(expr);
    }

    [Fact]
    public void Create2ShouldReturnTheGivenExpression()
    {
        Expression<Func<int, int, int>> expr = (a, b) => a;
        Expr.Create(expr).Should().BeSameAs(expr);
    }

    [Fact]
    public void Create3ShouldReturnTheGivenExpression()
    {
        Expression<Func<int, int, int, int>> expr = (a, b, c) => a;
        Expr.Create(expr).Should().BeSameAs(expr);
    }
}
