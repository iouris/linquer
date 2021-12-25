using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Linquer.Tests.Models;

public class ModelsContext : DbContext
{
    public DbSet<Person> People { get; set; } = null!;

    public ModelsContext(DbContextOptions options) : base(options)
    {
    }

    public static async Task<ModelsContext> CreateDefaultAsync()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            Mode = SqliteOpenMode.Memory,
            DataSource = "mydb"
        };
        var connectionString = connectionStringBuilder.ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlite(connectionString);

        var dbContext = new ModelsContext(optionsBuilder.Options);
        await dbContext.Database.OpenConnectionAsync();

        await dbContext.Database.EnsureCreatedAsync();

        var people = new Person[]
        {
            new() { Id = 1, Name = "John", Surname = "Smith", DateOfBirth = new(1990, 3, 15) },
            new() { Id = 2, Name = "Andrew", Surname = "Sparrow", DateOfBirth = new(2011, 2, 10) },
        };

        dbContext.People.AddRange(people);

        await dbContext.SaveChangesAsync();

        return dbContext;
    }
}
