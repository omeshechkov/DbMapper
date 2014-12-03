using System;

namespace DbMapper.Impl.Mappings.Xml.Exceptions
{
    public sealed class ParseValueException : Exception
    {
        public ParseValueException(object value, Type type)
            : base(string.Format("Cannot parse '{0}' value as '{1}' type", value ?? "null", type))
        {
        }
    }
}
