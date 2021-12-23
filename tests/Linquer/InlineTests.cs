using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xunit;

namespace Linquer.Tests;

using Models;

public class InlineTests
{
    [Fact]
    public async Task Inline_applied_to_lambda_with_another_inner_lambda_in_its_body_should_inline_the_inner_lambda_and_lead_to_successful_query()
    {
        var dbContext = await ModelsContext.CreateDefaultAsync();

        Expression<Func<Person, bool>> predicate = p => p.Name == "John";

        predicate = predicate.Or(p => p.Surname == "Sparrow");
        predicate = predicate.Inline();

        var query = dbContext.People.Where(predicate);

        var queryString = query.ToQueryString();
        queryString.Should().Contain(@"WHERE (""p"".""Name"" = 'John') OR (""p"".""Surname"" = 'Sparrow')");

        var people = await query.ToListAsync();

        people.Should().HaveCount(2);
        people.Should().Contain(p => p.Id == 1 && p.Name == "John" && p.Surname == "Smith");
        people.Should().Contain(p => p.Id == 2 && p.Name == "Andrew" && p.Surname == "Sparrow");
    }
}
