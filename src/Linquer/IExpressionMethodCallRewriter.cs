using System.Linq.Expressions;

namespace Linquer
{
    public interface IExpressionMethodCallRewriter
    {
        Expression? TryRewrite(MethodCallExpression methodCallExpression, Expression[] arguments);
    }
}