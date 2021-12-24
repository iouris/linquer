using Linquer.Internals;
using System.Linq.Expressions;

namespace Linquer
{
    public static class LinquerExtensions
    {
        public static Expression<TDelegate> Inline<TDelegate>(this Expression<TDelegate> predicate, IExpressionMethodCallRewriter? expressionMethodCallRewriter = null)
        {
            var expressionVisitor = new LinquerExpressionVisitor(expressionMethodCallRewriter);
            var inlined = expressionVisitor.Visit(predicate);
            return (Expression<TDelegate>)inlined;
        }
    }
}