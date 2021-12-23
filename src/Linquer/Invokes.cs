using System;
using System.Linq.Expressions;

namespace Linquer
{
    public static class Invokes
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static TRes Invoke<TArg1, TRes>(this Expression<Func<TArg1, TRes>> expression, TArg1 arg1) =>
            throw new NotImplementedException();

        public static TRes Invoke<TArg1, TArg2, TRes>(this Expression<Func<TArg1, TArg2, TRes>> expression, TArg1 arg1, TArg2 arg2) =>
            throw new NotImplementedException();

        public static TRes Invoke<TArg1, TArg2, TArg3, TRes>(this Expression<Func<TArg1, TArg2, TArg3, TRes>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3) =>
            throw new NotImplementedException();
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
