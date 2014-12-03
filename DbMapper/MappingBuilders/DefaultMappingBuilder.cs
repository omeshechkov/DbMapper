using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Exceptions;
using DbMapper.Mappings;

namespace DbMapper.MappingBuilders
{
    public abstract class DefaultMappingBuilder : IMappingBuilder
    {
        private readonly IDictionary<Type, IDictionary<Type, IMapping>> _mappings = new Dictionary<Type, IDictionary<Type, IMapping>>();

        public abstract void Configure(XElement configuration);

        public void RegisterMapping(IMapping mapping)
        {
            IDictionary<Type, IMapping> mappingTypes;
            if (!_mappings.TryGetValue(mapping.Type, out mappingTypes))
            {
                mappingTypes = new Dictionary<Type, IMapping>();
                _mappings[mapping.Type] = mappingTypes;
            }

            var mappingType = mapping is ITableViewMapping
                ? typeof(ITableViewMapping)
                : mapping.GetType();


            IMapping checkMapping;
            if (mappingTypes.TryGetValue(mappingType, out checkMapping))
                throw new ConfigurationException("Mapping of type '{0}' for '{1} type is already registered for '{2}' type",
                    mappingType.FullName, mapping.Type, checkMapping.Type);

            mappingTypes[mappingType] = mapping;
        }

        public TMapping GetMapping<TMapping>(Type type) where TMapping : IMapping
        {
            IDictionary<Type, IMapping> mappingTypes;
            if (!_mappings.TryGetValue(type, out mappingTypes))
                throw new Exception(string.Format("Cannot find mapping types for '{0}'", type));

            IMapping mapping;
            if (!mappingTypes.TryGetValue(typeof(TMapping), out mapping))
                throw new Exception(string.Format("Cannot find mapping type for '{0}'", type));

            return (TMapping)mapping;
        }
    }
}