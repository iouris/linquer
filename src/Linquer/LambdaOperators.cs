using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Linquer
{
    public static class LambdaOperators
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Expression<TDelegate>? CombineWith<TDelegate>(this Expression<TDelegate>? left, Expression<TDelegate>? right, Func<Expression<TDelegate>, Expression<TDelegate>, Expression<TDelegate>> combined) =>
            left == null ? right : right == null ? left : combined(left, right);

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, bool>>? And<TArg1>(this Expression<Func<TArg1, bool>>? left, Expression<Func<TArg1, bool>>? right) =>
            left.CombineWith(right, (l, r) => arg1 => l.Invoke(arg1) && r.Invoke(arg1));

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, TArg2, bool>>? And<TArg1, TArg2>(this Expression<Func<TArg1, TArg2, bool>>? left, Expression<Func<TArg1, TArg2, bool>>? right) =>
            left.CombineWith(right, (l, r) => (arg1, arg2) => l.Invoke(arg1, arg2) && r.Invoke(arg1, arg2));

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, TArg2, TArg3, bool>>? And<TArg1, TArg2, TArg3>(this Expression<Func<TArg1, TArg2, TArg3, bool>>? left, Expression<Func<TArg1, TArg2, TArg3, bool>>? right) =>
            left.CombineWith(right, (l, r) => (arg1, arg2, arg3) => l.Invoke(arg1, arg2, arg3) && r.Invoke(arg1, arg2, arg3));

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, bool>>? Or<TArg1>(this Expression<Func<TArg1, bool>>? left, Expression<Func<TArg1, bool>>? right) =>
            left.CombineWith(right, (l, r) => arg1 => l.Invoke(arg1) || r.Invoke(arg1));

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, TArg2, bool>>? Or<TArg1, TArg2>(this Expression<Func<TArg1, TArg2, bool>>? left, Expression<Func<TArg1, TArg2, bool>>? right) =>
            left.CombineWith(right, (l, r) => (arg1, arg2) => l.Invoke(arg1, arg2) || r.Invoke(arg1, arg2));

        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static Expression<Func<TArg1, TArg2, TArg3, bool>>? Or<TArg1, TArg2, TArg3>(this Expression<Func<TArg1, TArg2, TArg3, bool>>? left, Expression<Func<TArg1, TArg2, TArg3, bool>>? right) =>
            left.CombineWith(right, (l, r) => (arg1, arg2, arg3) => l.Invoke(arg1, arg2, arg3) || r.Invoke(arg1, arg2, arg3));
    }
}