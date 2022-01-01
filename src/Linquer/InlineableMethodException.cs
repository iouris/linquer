using System;

namespace Linquer
{
    public class InlineableMethodException : Exception
    {
        public InlineableMethodException(string methodClassName, string methodName) :
            base($"{methodClassName}.{methodName} method should not be called, it is expected to be used in LINQ expressions only.")
        {
        }
    }
}
