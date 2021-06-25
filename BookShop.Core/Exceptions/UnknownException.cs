using System;

namespace BookShop.Core.Exceptions
{
    public class UnknownException : Exception
    {
        public UnknownException(string message) : base(message) { }
    }
}
