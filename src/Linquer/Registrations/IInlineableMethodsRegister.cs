using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Linquer.Registrations
{
    public interface IInlineableMethodsRegister
    {
        void Register(MethodInfo method, Func<Expression[], Expression> inlinedMethodFactory);
    }
}
