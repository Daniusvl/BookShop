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

        public NotFoundException(string paramName, string id)
        {
            ParamName = paramName;
            Id = id;
        }

        public string ParamName { get; }
        public object Id { get; }

        public override string ToString()
        {
            return nameof(NotFoundException) + $": {ParamName}, {Id}";
        }
    }
}
