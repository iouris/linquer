using FluentAssertions;
using Linquer.Registrations;
using Linquer.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xunit;

namespace Linquer.Tests;

using PersonPredicate = Expression<Func<Person, bool>>;

public class LinquerExtensionsTests
{
    [Fact]
    public async Task Inline_applied_to_lambda_with_another_inner_lambda_in_its_body_should_inline_the_inner_lambda_and_lead_to_successful_query()
    {
        var dbContext = await ModelsContext.CreateDefaultAsync();

        PersonPredicate predicate = p => p.Name == "John";

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

    [Fact]
    public async Task Inline_should_not_attempt_to_inline_known_function()
    {
        var dbContext = await ModelsContext.CreateDefaultAsync();

        PersonPredicate predicate = p => !string.IsNullOrEmpty(p.Name);
        predicate = predicate.Inline();

        var query = dbContext.People.Where(predicate);

        var queryString = query.ToQueryString();
        queryString.Should().Contain(@"WHERE ""p"".""Name"" <> ''");

        var people = await query.ToListAsync();

        people.Should().HaveCount(2);
        people.Should().Contain(p => p.Name == "John");
        people.Should().Contain(p => p.Name == "Andrew");
    }

    private static IInlineableMethodsProvider CreateCustomExpressionsRegister()
    {
        var register = new InlineableMethodsRegister(ExpressionMethodCallRewriter.DefaultInlineableMethodsProvider);
        register.Register(() => Dates.CurrentYear(), () => 2021);
        register.Register((Person p) => p.Age(), p => Dates.CurrentYear() - p.DateOfBirth.Year);
        return register;
    }

    private static readonly IExpressionMethodCallRewriter CustomExpressionMethodCallRewriter = new ExpressionMethodCallRewriter(
        CreateCustomExpressionsRegister()
    );

    [Fact]
    public async Task Inline_should_honour_custom_IExpressionMethodCallRewriter()
    {
        var dbContext = await ModelsContext.CreateDefaultAsync();

        var adultAge = Expr.Create(() => 18);
        PersonPredicate personIsAdult = person => person.Age() >= adultAge.Invoke();
        personIsAdult = personIsAdult.Inline(CustomExpressionMethodCallRewriter);

        var query = dbContext.People.Where(personIsAdult);

        var people = await query.ToListAsync();

        people.Should().HaveCount(1);
        people.Should().Contain(p => p.Name == "John" && p.Surname == "Smith");
    }
}