using FluentAssertions;
using Xunit;

namespace Linquer.Tests;

public class InvokesTests
{
    [Fact]
    public void InvokeWithNoArgumentsShouldThrow()
    {
        var invoke = () => Invokes.Invoke(() => 0);
        invoke.Should().ThrowExactly<InlineableMethodException>().WithMessage("*should not be called*");
    }

    [Fact]
    public void InvokeWith1ArgumentShouldThrow()
    {
        var invoke = () => Invokes.Invoke((int _) => 0, default);
        invoke.Should().ThrowExactly<InlineableMethodException>().WithMessage("*should not be called*");
    }

    [Fact]
    public void InvokeWith2ArgumentsShouldThrow()
    {
        var invoke = () => Invokes.Invoke((int _, int _) => 0, default, default);
        invoke.Should().ThrowExactly<InlineableMethodException>().WithMessage("*should not be called*");
    }

    [Fact]
    public void InvokeWith3ArgumentsShouldThrow()
    {
        var invoke = () => Invokes.Invoke((int _, int _, int _) => 0, default, default, default);
        invoke.Should().ThrowExactly<InlineableMethodException>().WithMessage("*should not be called*");
    }
}
