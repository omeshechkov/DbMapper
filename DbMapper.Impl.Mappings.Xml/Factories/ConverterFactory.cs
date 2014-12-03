using System;
using System.Collections.Generic;
using DbMapper.Impl.Mappings.Xml.Exceptions;

namespace DbMapper.Impl.Mappings.Xml.Factories
{
    public static class ConverterFactory
    {
        private static readonly IDictionary<string, IConverter> Shorthands = new Dictionary<string, IConverter>();
        
        private static readonly IDictionary<Type, IConverter> Cache = new Dictionary<Type, IConverter>();

        public static void RegisterShorthand<T>(string name) where T : IConverter, new()
        {
            Shorthands[name] = new T();
        }

        public static IConverter Create(string converterTypeString)
        {
            if (converterTypeString == null)
                throw new ArgumentNullException("converterTypeString");

            IConverter converter;
            if (Shorthands.TryGetValue(converterTypeString, out converter))
                return converter;

            try
            {
                var converterType = Type.GetType(converterTypeString, true);

                if (Cache.TryGetValue(converterType, out converter))
                    return converter;

                if (!typeof(IConverter).IsAssignableFrom(converterType))
                    throw new DocumentParseException("Illegal converter class '{0}', class must be inherited from '{1}'", converterType.FullName, typeof(IConverter).FullName);

                converter = (IConverter)Activator.CreateInstance(converterType);

                Cache[converterType] = converter;

                return converter;
            }
            catch (TypeLoadException ex)
            {
                throw new DocumentParseException("Cannot load converter", ex);
            }
        }
    }
}