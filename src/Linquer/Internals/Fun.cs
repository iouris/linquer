using System;

namespace Linquer.Internals
{
    internal static class Fun
    {
        public static TRes Invoke<TRes>(Func<TRes> func) => func();
    }
}
