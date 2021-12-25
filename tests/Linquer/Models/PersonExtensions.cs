namespace Linquer.Tests.Models;

public static class PersonExtensions
{
    public static int Age(this Person _) =>
        throw new NotSupportedException($"The {nameof(Age)}() method should not be called, it is expected to be used in LINQ expressions only.");
}
