﻿using System;
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
                Type converterType;

                try
                {
                    converterType = Type.GetType(converterTypeString, true);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot parse converter type, unrecognized class '{0}'", converterTypeString), ex);
                }

                if (Cache.TryGetValue(converterType, out converter))
                    return converter;

                if (!typeof(IConverter).IsAssignableFrom(converterType))
                    throw new DocumentParseException("Illegal converter class '{0}', class must be inherited from '{1}'", 
                        converterType.AssemblyQualifiedName, typeof(IConverter).AssemblyQualifiedName);

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