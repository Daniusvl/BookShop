using System;

namespace BookShop.Core.Exceptions
{
    public class CommonException : Exception
    {
        public CommonException(string message) : base(message) { }
    }
}
