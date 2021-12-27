# linquer

[![Coverage Status](https://coveralls.io/repos/github/iouris/linquer/badge.svg?branch=main)](https://coveralls.io/github/iouris/linquer?branch=main)

# About this project
A .NET Standard 2.1 library to allow dynamic composition of LINQ predicates.

# Who created this?
Originally authored by Iouri S.

# Usage examples

## Dynamic composition of LINQ predicates

### Problem
Often we need to be able to build the filtering predicate used in **IQueryable.Where()** method dynamically, based on the various conditions or switches. 

The common approach when building a predicate with conditions all of which need to be met is to apply the **Where()** method several times. For example:

```C#
var query = dbContext.People.Where(p => p.Name == "John");
if(includeSurname)
    query = query.Where(p => p.Surname == "Sparrow")

var people = query.ToList();
``` 

It is difficult however to build a predicate where either of the conditions are to be met. An often used approach which can lead to a sub-optimal SQL query is to include the switch value in the query. For example:

```C#
var query = dbContext.People.Where(
    p => p.Name == "John" || (includeSurname && p.Surname == "Sparrow")
);

var people = query.ToList();
```

### Solution

Linquer helps with this case by providing a way of building the LINQ predicates dynamically, in a strongly typed manner. 

```C#
using Linquer;
...

Expression<Func<Person, bool>> predicate = p.Name == "John";
if(includeSurname)
    predicate = predicate.Or(p => p.Surname == "Sparrow");

predicate = predicate.Inline();

var query = dbContext.People.Where(predicate);
var people = query.ToList();
```

Note that before a dynamically built expression can be passed to the IQueryable.Where() method, in needs to be 'inlined' through the **Inline()** call.

Both **And()** and **Or()** extension methods are available to build LINQ expressions dynamically. Please see the unit tests for more details.

## Refactoring LINQ expressions into reusable functions 

### Problem
Often we come across scenarious where we would like to be able to refactor a commonly used LINQ expression into a reusable function. For example, given that our Person class has the DateOfBirth property, we would like to extend this class with **Age()** method which returns the person's age in full years. This would allow us instead of repeating the logic across our project as so

```C#
var people = dbContext.Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) >= 18);
```

use our refactored logic as so

```C#
var people = dbContext.Where(p => p.Age() >= 18);
```

### Solution

Linquer's Inline() extension method has a default parameter: **IExpressionMethodCallRewriter? expressionMethodCallRewriter = null**

The **IExpressionMethodCallRewriter** interface is defined as 

```C#
public interface IExpressionMethodCallRewriter
{
    Expression? TryRewrite(MethodCallExpression methodCallExpression, Expression[] arguments);
}
```

You can have your own project-wide implementation of it in the following manner:

```C#
public static class PersonExtensions
{
    public static int Age(this Person _) =>
        throw new NotSupportedException($"The {nameof(Age)}() method should not be called, it is expected to be used in LINQ expressions only.");
}

...

private static IInlineableMethodsProvider CreateCustomExpressionsRegister()
{
    var register = new InlineableMethodsRegister(ExpressionMethodCallRewriter.DefaultInlineableMethodsProvider);
    register.Register((Person p) => p.Age(), p => DateTime.Now.Year - p.DateOfBirth.Year);
    return register;
}

private static readonly IExpressionMethodCallRewriter CustomExpressionMethodCallRewriter = new ExpressionMethodCallRewriter(
    CreateCustomExpressionsRegister()
);
```

With the above in place, you can use the **Age()** extension method in your LINQ expressions:

```C#
Expression<Func<Person, bool>> predicate = p.Age() >= 18;
predicate = predicate.Inline(CustomExpressionMethodCallRewriter);

var query = dbContext.People.Where(predicate);
var people = query.ToList();
```

A reminder that you need to apply the **Inline** to an expression before it can be passed to the **IQueryable.Where()** method. 