using Linquer.Registrations;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Linquer
{
    public static class Invokes
    {
#pragma warning disable IDE0060 // Remove unused parameter
        [DoesNotReturn]
        public static TRes Invoke<TRes>(this Expression<Func<TRes>> expression) =>
            throw ToBeUsedInLinqExpressionsOnly();

        [DoesNotReturn]
        public static TRes Invoke<TArg1, TRes>(this Expression<Func<TArg1, TRes>> expression, TArg1 arg1) =>
            throw ToBeUsedInLinqExpressionsOnly();

        [DoesNotReturn]
        public static TRes Invoke<TArg1, TArg2, TRes>(this Expression<Func<TArg1, TArg2, TRes>> expression, TArg1 arg1, TArg2 arg2) =>
            throw ToBeUsedInLinqExpressionsOnly();

        [DoesNotReturn]
        public static TRes Invoke<TArg1, TArg2, TArg3, TRes>(this Expression<Func<TArg1, TArg2, TArg3, TRes>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3) =>
            throw ToBeUsedInLinqExpressionsOnly();

        private static InlineableMethodException ToBeUsedInLinqExpressionsOnly() =>
            new InlineableMethodException(nameof(Invokes), nameof(Invoke));

#pragma warning restore IDE0060 // Remove unused parameter

        public static void RegisterInlineableInvokes(this IInlineableMethodsRegister inlineableMethodsRegister)
        {
            static Expression createInvokeExpression(Expression[] args) =>
                Expression.Invoke(args[0], args.Skip(1).ToArray());

            inlineableMethodsRegister.Register(() => Invokes.Invoke<int>(default!), createInvokeExpression);
            inlineableMethodsRegister.Register(() => Invokes.Invoke<int, int>(default!, default), createInvokeExpression);
            inlineableMethodsRegister.Register(() => Invokes.Invoke<int, int, int>(default!, default, default), createInvokeExpression);
            inlineableMethodsRegister.Register(() => Invokes.Invoke<int, int, int, int>(default!, default, default, default), createInvokeExpression);
        }
    }
}
