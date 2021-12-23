using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Linquer.Internals
{
    internal class LinquerExpressionVisitor : ExpressionVisitor
    {
        private class ExpressionsCallStack
        {
            private readonly Dictionary<ParameterExpression, Expression> ParameterValueLookup =
                new Dictionary<ParameterExpression, Expression>();

            private readonly Stack<IEnumerable<ParameterExpression>> LambdaCallFrames =
                new Stack<IEnumerable<ParameterExpression>>();

            public void Enter(IEnumerable<ParameterExpression> parameters, IEnumerable<Expression> arguments)
            {
                parameters.Zip(arguments, (p, arg) => (p, arg)).Iter(a => this.ParameterValueLookup.Add(a.p, a.arg));
                this.LambdaCallFrames.Push(parameters);
            }

            public void Leave() =>
                this.LambdaCallFrames.Pop().Iter(p => this.ParameterValueLookup.Remove(p));

            public Expression? TryFindParameterValue(ParameterExpression parameter) =>
                this.ParameterValueLookup.GetValueOrDefault(parameter) switch
                {
                    ParameterExpression anotherParameter => this.TryFindParameterValue(anotherParameter),
                    var value => value
                };
        }

        private readonly IExpressionMethodCallRewriter ExpressionMethodCallRewriter;
        private readonly ExpressionsCallStack CallStack = new ExpressionsCallStack();

        public LinquerExpressionVisitor(IExpressionMethodCallRewriter? expressionMethodCallRewriter)
        {
            this.ExpressionMethodCallRewriter = expressionMethodCallRewriter ?? Internals.ExpressionMethodCallRewriter.Instance;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            var rewrittenArguments = m.Arguments.Select(arg => this.Visit(arg)).ToArray();
            var substitute = this.ExpressionMethodCallRewriter.TryRewrite(m, rewrittenArguments);

            var rewritten = substitute switch
            {
                LambdaExpression lambda =>
                    this.InlineLambda(lambda, rewrittenArguments),

                Expression expression =>
                    base.Visit(expression),

                null =>
                    base.VisitMethodCall(m)
            };
            return rewritten;
        }

        protected override Expression VisitParameter(ParameterExpression p) =>
            this.CallStack.TryFindParameterValue(p) ?? p;

        private Expression InlineLambdaBody(Expression body, IEnumerable<ParameterExpression> parameters, Expression[] arguments)
        {
            this.CallStack.Enter(parameters, arguments);
            var expanded = this.Visit(body);
            this.CallStack.Leave();
            return expanded;
        }

        private Expression InlineLambda(LambdaExpression lambda, Expression[] arguments) =>
            this.InlineLambdaBody(lambda.Body, lambda.Parameters, arguments);
    }
}