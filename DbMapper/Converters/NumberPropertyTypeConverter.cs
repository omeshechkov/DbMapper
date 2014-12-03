using System;
using DbMapper.Converters.Exceptions;

namespace DbMapper.Converters
{
    public abstract class Converter<T1, T2> : IConverter
    {
        public Type Type1
        {
            get { return typeof(T1); }
        }

        public Type Type2
        {
            get { return typeof(T2); }
        }

        public bool CanConvert(Type t1, Type t2)
        {
            return (t1 == typeof(T1) && t2 == typeof(T2)) || (t1 == typeof(T2) && t2 == typeof(T1));
        }

        public Delegate GetConverter(Type sourceType, Type targetType)
        {
            if (sourceType == typeof(T1) && targetType == typeof(T2))
                return ConverterFromT1ToT2;

            if (sourceType == typeof(T2) && targetType == typeof(T1))
                return ConverterFromT2ToT1;

            throw new ConversionException(sourceType, targetType);
        }

        protected abstract Func<T1, T2> ConverterFromT1ToT2 { get; }

        protected abstract Func<T2, T1> ConverterFromT2ToT1 { get; }
    }
}