namespace Linquer.Tests.Models;

public static class Dates
{
    public static int CurrentYear() =>
        throw new InlineableMethodException(nameof(Dates), nameof(CurrentYear));
}
