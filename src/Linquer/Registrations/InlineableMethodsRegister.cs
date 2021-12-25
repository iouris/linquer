using Linquer.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer.Registrations
{
    public class InlineableMethodsRegister : IInlineableMethodsRegister, IInlineableMethodsProvider
    {
        private readonly Dictionary<MethodInfo, Func<Expression[], Expression>> InlinedMethodFactories =
            new Dictionary<MethodInfo, Func<Expression[], Expression>>();

        public InlineableMethodsRegister(IInlineableMethodsProvider? inlineableMethodsProvider = null)
        {
            inlineableMethodsProvider?.InlineableMethods.Iter(m => this.InlinedMethodFactories.Add(m.method, m.inlinedMethodFactory));
        }

        public IEnumerable<(MethodInfo method, Func<Expression[], Expression> inlinedMethodFactory)> InlineableMethods =>
            this.InlinedMethodFactories.Select(kvp => (kvp.Key, kvp.Value));

        public void Register(MethodInfo method, Func<Expression[], Expression> inlinedMethodFactory) =>
            this.InlinedMethodFactories.Add(method, inlinedMethodFactory);

        public Func<Expression[], Expression>? TryFindInlineableMethodFactory(MethodInfo method) =>
            this.InlinedMethodFactories.GetValueOrDefault(method);
    }
}
