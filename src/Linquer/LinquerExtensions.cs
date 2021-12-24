using Linquer.Internals;
using System;
using System.Linq.Expressions;

namespace Linquer
{
    public static class LinquerExtensions
    {
        public static Expression<Func<TArg1, bool>> Inline<TArg1>(this Expression<Func<TArg1, bool>> predicate, IExpressionMethodCallRewriter? expressionMethodCallRewriter = null)
        {
            var expressionVisitor = new LinquerExpressionVisitor(expressionMethodCallRewriter);
            var inlined = expressionVisitor.VisitAndConvert(predicate, callerName: nameof(Inline));
            return inlined;
        }
    }
}