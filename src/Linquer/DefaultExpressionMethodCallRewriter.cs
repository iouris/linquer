using System.Linq;
using System.Linq.Expressions;

namespace Linquer
{
    public class DefaultExpressionMethodCallRewriter : IExpressionMethodCallRewriter
    {
        public static readonly DefaultExpressionMethodCallRewriter Instance = new DefaultExpressionMethodCallRewriter();

        public Expression? TryRewrite(MethodCallExpression methodCallExpression, Expression[] arguments)
        {
            var createInvocationExpression =
                methodCallExpression.Method.Name == nameof(Invokes.Invoke) &&
                arguments.Any() &&
                typeof(LambdaExpression).IsAssignableFrom(arguments[0].Type);

            if (createInvocationExpression)
                return Expression.Invoke(arguments[0], arguments.Skip(1).ToArray());

            return null;
        }
    }
}
