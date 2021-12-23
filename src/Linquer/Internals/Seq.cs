using System;
using System.Collections.Generic;

namespace Linquer.Internals
{
    internal static class Seq
    {
        public static void Iter<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }
}
