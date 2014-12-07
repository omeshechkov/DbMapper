using System;

namespace DbMapper.MappingValidators.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public ValidationException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
