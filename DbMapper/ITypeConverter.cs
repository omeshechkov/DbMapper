using System;

namespace DbMapper
{
    public interface IConverter
    {
        Type Type1 { get; }

        Type Type2 { get; }

        bool CanConvert(Type t1, Type t2);

        Delegate GetConverter(Type sourceType, Type targetType);
    }

    public interface IHasConverter
    {
        /// <summary>
        /// It's converter from type returned from data reader to property type, can be assigned during the first execution
        /// </summary>
        IConverter Converter { get; set; }
    }
}