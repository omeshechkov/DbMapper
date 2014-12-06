using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Exceptions;
using DbMapper.Mappings;

namespace DbMapper.MappingBuilders
{
    public abstract class DefaultMappingBuilder : IMappingBuilder
    {
        private readonly IDictionary<Type, IMappingClassReference> _mappings = new Dictionary<Type, IMappingClassReference>();

        public abstract void Configure(XElement configuration);

        public void RegisterMapping(IMappingClassReference mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            IMappingClassReference checkMapping;
            if (_mappings.TryGetValue(mapping.Type, out checkMapping))
                throw new ConfigurationException("Mapping of type '{0}' for '{1} type is already registered for '{2}' type",
                    mapping.GetType().FullName, mapping.Type.FullName, checkMapping.Type.FullName);

            _mappings[mapping.Type] = mapping;
        }

        public TMapping GetMapping<TMapping>(Type type) where TMapping : IMappingClassReference
        {
            if (type == null)
                throw new ArgumentNullException("type");

            IMappingClassReference mapping;
            if (!_mappings.TryGetValue(type, out mapping))
                throw new Exception(string.Format("Cannot find mapping type for '{0}'", type.FullName));

            if (!typeof(TMapping).IsAssignableFrom(type))
                throw new Exception(string.Format("Wrong mapping type for '{0}', expected: '{1}', actual '{2}'", 
                    type.FullName, typeof(TMapping).FullName, mapping.GetType().FullName));

            return (TMapping)mapping;
        }
    }
}