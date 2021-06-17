using System;

namespace BookShop.Core.Exceptions
{
    public class ServiceNullException : Exception
    {
        public ServiceNullException(string paramName, string where)
        {
            ParamName = paramName;
            Where = where;
        }

        public string ParamName { get; }

        public string Where { get; }
    }
}
