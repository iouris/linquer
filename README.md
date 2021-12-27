# linquer

[![Coverage Status](https://coveralls.io/repos/github/iouris/linquer/badge.svg?branch=main)](https://coveralls.io/github/iouris/linquer?branch=main)

# About this project
A .NET Standard 2.1 library to allow dynamic composition of LINQ predicates.

# Who created this?
Originally authored by Iouri S.

# Usage examples

## Dynamic composition of LINQ predicates

### Problem
Often we need to be able to build the filtering predicate used in IQueryable<>.Where() method dynamically, based on the various conditions or switches. 

The common approach when building a predicate with conditions all of which need to be met is to apply the Where() method several times. For example:

```
var query = dbContext.People.Where(p => p.Name == "John");
if(includeSurname)
    query = query.Where(p => p.Surname == "Sparrow")

var people = query.ToList();
``` 

It is difficult however to build a predicate where either of the conditions are to be met. An often used approach which can lead to a sub-optimal SQL query is to include the switch value in the query. For example:

```
var query = dbContext.People.Where(p => p.Name == "John" || (includeSurname && p.Surname == "Sparrow"));

var people = query.ToList();
```

### Solution

Linquer helps with this case by providing a way of building the LINQ predicates dynamically, in a strongly typed manner. 

```
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