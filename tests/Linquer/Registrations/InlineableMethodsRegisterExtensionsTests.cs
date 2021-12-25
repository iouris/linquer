using FluentAssertions;
using Linquer.Registrations;
using Xunit;

namespace Linquer.Tests.Registrations;

public class InlineableMethodsRegisterExtensionsTests
{
    [Fact]
    public void Register_with_no_method_call_in_the_body_should_throw() =>
        new Action(() => new InlineableMethodsRegister().Register(() => 1, _ => default!))
        .Should().Throw<ArgumentException>().WithMessage("*expected to contain a method call*");
}
