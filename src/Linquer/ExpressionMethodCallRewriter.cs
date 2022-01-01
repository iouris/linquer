using Linquer.Internals;
using Linquer.Registrations;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer
{
    public class ExpressionMethodCallRewriter : IExpressionMethodCallRewriter
    {
        public static readonly IInlineableMethodsProvider DefaultInlineableMethodsProvider = Fun.Invoke(() =>
        {
            var register = new InlineableMethodsRegister();
            register.RegisterInlineableInvokes();
            return register;
        });

        public static readonly ExpressionMethodCallRewriter Default =
            new ExpressionMethodCallRewriter(DefaultInlineableMethodsProvider);

        private readonly IInlineableMethodsProvider InlineableMethodsProvider;

        public ExpressionMethodCallRewriter(IInlineableMethodsProvider inlineableMethodsProvider)
        {
            this.InlineableMethodsProvider = inlineableMethodsProvider;
        }

        public Expression? TryRewrite(MethodCallExpression methodCallExpression, Expression[] arguments)
        {
            var inlinedMethodFactory = TryFindInlineableMethodFactory(methodCallExpression);
            var inlinedMethod = inlinedMethodFactory?.Invoke(arguments);
            return inlinedMethod;
        }

        private Func<Expression[], Expression>? TryFindInlineableMethodFactory(MethodCallExpression methodCallExpression)
        {
            Func<Expression[], Expression>? tryFind(MethodInfo method) =>
                this.InlineableMethodsProvider.TryFindInlineableMethodFactory(method);

            var method = methodCallExpression.Method;
            var inlinedMethodFactory = tryFind(method) switch
            {
                null when method.IsGenericMethod => tryFind(method.GetGenericMethodDefinition()),
                var other => other
            };
            return inlinedMethodFactory;
        }
    }
}
