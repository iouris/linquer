using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer
{
    public interface IInlineableMethodsProvider
    {
        Func<Expression[], Expression>? TryFindInlineableMethodFactory(MethodInfo method);
        IEnumerable<(MethodInfo method, Func<Expression[], Expression> inlinedMethodFactory)> InlineableMethods { get; }
    }
}
