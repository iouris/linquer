using Linquer.Internals;
using Linquer.Registrations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer
{
    public class ExpressionMethodCallRewriter : IExpressionMethodCallRewriter
    {
        public static readonly IInlineableMethodsProvider DefaultInlineableMethodsProvider = Fun.Invoke(() =>
        {
            static Expression createInvokeExpression(Expression[] args) =>
                Expression.Invoke(args[0], args.Skip(1).ToArray());

            var register = new InlineableMethodsRegister();
            register.Register(() => Invokes.Invoke<int, int>(default!, default), createInvokeExpression);
            register.Register(() => Invokes.Invoke<int, int, int>(default!, default, default), createInvokeExpression);
            register.Register(() => Invokes.Invoke<int, int, int, int>(default!, default, default, default), createInvokeExpression);
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
