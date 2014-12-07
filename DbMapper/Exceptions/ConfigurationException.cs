using System;

namespace DbMapper.Exceptions
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
