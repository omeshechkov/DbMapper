using System;

namespace DbMapper.Impl.Mappings.Xml.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ConfigurationException(string format, params object[] args) : base(string.Format(format, args))
        {
        }
    }
}
