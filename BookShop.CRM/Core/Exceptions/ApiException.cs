using System;

namespace BookShop.CRM.Core.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string msg) : base(msg) { }
    }
}
