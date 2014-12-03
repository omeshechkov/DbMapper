using System;

namespace DbMapper.Impl.Mappings.Xml.Exceptions
{
    public class DocumentParseException : Exception
    {
        public DocumentParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DocumentParseException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
