using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Linquer
{
    public static class Expr
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TRes>> Create<TRes>(Expression<Func<TRes>> expr) => expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TArg1, TRes>> Create<TArg1, TRes>(Expression<Func<TArg1, TRes>> expr) => expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TArg1, TArg2, TRes>> Create<TArg1, TArg2, TRes>(Expression<Func<TArg1, TArg2, TRes>> expr) => expr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TArg1, TArg2, TArg3, TRes>> Create<TArg1, TArg2, TArg3, TRes>(Expression<Func<TArg1, TArg2, TArg3, TRes>> expr) => expr;
    }
}