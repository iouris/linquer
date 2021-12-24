using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xunit;
using System.Reflection;
using Linquer.Tests.Models;

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

    private class CustomExpressionMethodCallRewriter : IExpressionMethodCallRewriter
    {
        public static readonly IExpressionMethodCallRewriter Instance = new CustomExpressionMethodCallRewriter();

        private const int CurrentYear = 2021;
        private static readonly Expression<Func<Person, int>> AgeExpression = p => CurrentYear - p.DateOfBirth.Year;

        public Expression? TryRewrite(MethodCallExpression methodCallExpression, Expression[] arguments)
        {
            var rewritten = DefaultExpressionMethodCallRewriter.Instance.TryRewrite(methodCallExpression, arguments);
            if (rewritten != null)
                return rewritten;

            var method = methodCallExpression.Method;
            if (method == InlineableFunctions.AgeMethod)
                return AgeExpression;

            return null;
        }
    }

    private static class InlineableFunctions
    {
        public static int Age(Person _) =>
            throw new NotSupportedException("it is not intended to be called.");

        public static readonly MethodInfo AgeMethod = typeof(InlineableFunctions).GetMethod(nameof(Age))!;
    }

    [Fact]
    public async Task Inline_should_honour_custom_IExpressionMethodCallRewriter()
    {
        var dbContext = await ModelsContext.CreateDefaultAsync();

        PersonPredicate atLeast30YearsOld = person => InlineableFunctions.Age(person) > 30;
        atLeast30YearsOld = atLeast30YearsOld.Inline(CustomExpressionMethodCallRewriter.Instance);

        var query = dbContext.People.Where(atLeast30YearsOld);

        var people = await query.ToListAsync();

        people.Should().HaveCount(1);
        people.Should().Contain(p => p.Name == "John" && p.Surname == "Smith");
    }
}