using System;

namespace BookShop.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string paramName, int id)
        {
            ParamName = paramName;
            Id = id;
        }

        public string ParamName { get; }
        public int Id { get; }
    }
}
