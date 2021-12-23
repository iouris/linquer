using System;
using System.Linq.Expressions;

namespace Linquer
{
    public static class Expr
    {
        public static Expression<Func<TArg1, TRes>> Create<TArg1, TRes>(Expression<Func<TArg1, TRes>> expr) => expr;
    }
}