using System;
using System.Linq.Expressions;
using Linquer.Internals;

namespace Linquer
{
    public static class LambdaExtensions
    {
        public static Expression<Func<TArg1, bool>> Inline<TArg1>(this Expression<Func<TArg1, bool>> predicate, IExpressionMethodCallRewriter? expressionMethodCallRewriter = null)
        {
            var expressionVisitor = new LinquerExpressionVisitor(expressionMethodCallRewriter);
            return expressionVisitor.VisitAndConvert(predicate, callerName: nameof(Inline));
        }
    }
}