namespace Linquer.Tests.Models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
