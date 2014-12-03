using System;

namespace DbMapper.Converters.Exceptions
{
    public class ConversionException : Exception
    {
        public ConversionException(Type type1, Type type2) : base(string.Format("Cannot convert from '{0}' type to '{1}'", type1.FullName, type2.FullName))
        {
        }
    }
}
