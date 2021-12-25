using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer.Registrations
{
    public static class InlineableMethodsRegisterExtensions
    {
        public static void Register(
            this IInlineableMethodsRegister inlineableMethodsRegister,
            LambdaExpression inlineableMethodContainer,
            Func<Expression[], Expression> inlinedMethodFactory
        )
        {
            var method = inlineableMethodContainer.GetInlineableMethod();
            if (method.IsGenericMethod)
                method = method.GetGenericMethodDefinition();

            inlineableMethodsRegister.Register(method, inlinedMethodFactory);
        }

        public static void Register<TRes>(
            this IInlineableMethodsRegister inlineableMethodsRegister,
            Expression<Func<TRes>> inlineableMethodContainer,
            Func<Expression[], Expression> inlinedMethodFactory
        ) =>
            inlineableMethodsRegister.Register((LambdaExpression)inlineableMethodContainer, inlinedMethodFactory);

        public static void Register<TArg1, TRes>(
            this IInlineableMethodsRegister inlineableMethodsRegister,
            Expression<Func<TArg1, TRes>> inlineableMethodContainer,
            Expression<Func<TArg1, TRes>> inlinedMethod
        ) =>
            inlineableMethodsRegister.Register(inlineableMethodContainer, _ => inlinedMethod);

        private static MethodInfo GetInlineableMethod(this LambdaExpression inlineableMethodContainer) =>
            (inlineableMethodContainer.Body as MethodCallExpression)?.Method ??
            throw new ArgumentException($"{nameof(inlineableMethodContainer)} is expected to contain a method call.");
    }
}
