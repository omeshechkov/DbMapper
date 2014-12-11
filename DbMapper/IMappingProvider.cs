using System;
using System.Collections.Generic;
using System.Linq;
using DbMapper.Exceptions;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper
{
    public interface IMappingProvider
    {
        TMapping GetMapping<TMapping>(Type type) where TMapping : IMappingClassReference;
    }

    internal class MappingProvider : IMappingProvider
    {
        private readonly IMappingValidatorFactory _mappingValidatorFactory;

        private readonly IList<IDynamicMappingBuilder> _dynamicMappingBuilders = new List<IDynamicMappingBuilder>();
        private readonly IDictionary<Type, IMappingClassReference> _mappings = new Dictionary<Type, IMappingClassReference>();

        public MappingProvider(IMappingValidatorFactory mappingValidatorFactory)
        {
            _mappingValidatorFactory = mappingValidatorFactory;
        }

        internal void RegisterMappingBuilder(IMappingBuilder mappingBuilder)
        {
            var dynamicMappingBuilder = mappingBuilder as IDynamicMappingBuilder;
            if (dynamicMappingBuilder != null)
            {
                _dynamicMappingBuilders.Add(dynamicMappingBuilder);
            }

            var staticMappingBuilder = mappingBuilder as IStaticMappingBuilder;
            if (staticMappingBuilder != null)
            {
                foreach (var mapping in staticMappingBuilder.Mappings)
                {
                    if (mapping.Type == null)
                        throw new ConfigurationException("Cannot load mapping of type '{0}', type is null", mapping.GetType().AssemblyQualifiedName);

                    IMappingClassReference curMapping;
                    if (_mappings.TryGetValue(mapping.Type, out curMapping))
                        throw new ConfigurationException("Cannot load mapping of type '{0}, type is already registered for '{1}'", mapping.Type.AssemblyQualifiedName, curMapping.GetType().AssemblyQualifiedName);

                    _mappings.Add(mapping.Type, mapping);
                }
            }
        }

        internal void Initialize()
        {
            using (var validationContext = new ValidationContext<IMappingClassReference>(_mappingValidatorFactory))
            {
                foreach (var mapping in _mappings.Values)
                {
                    validationContext.Validate(mapping);
                }
            }

            GC.Collect();
        }

        public TMapping GetMapping<TMapping>(Type type) where TMapping : IMappingClassReference
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var @dynamic = false;

            IMappingClassReference mapping;
            if (!_mappings.TryGetValue(type, out mapping))
            {
                var tmp = default(TMapping);

                @dynamic = _dynamicMappingBuilders.Any(b => b.TryGetMapping(type, out tmp));

                mapping = tmp;
            }

            if (mapping == null)
                throw new Exception(string.Format("Cannot get mapping for type '{0}', mapping not found", type.AssemblyQualifiedName));

            if (!(mapping is TMapping))
            {
                throw new Exception(string.Format("Cannot get mapping for type '{0}', expected mapping of type is '{1}', actual: {2}",
                    type.AssemblyQualifiedName, typeof(TMapping).AssemblyQualifiedName, mapping.GetType().AssemblyQualifiedName));
            }


            if (!@dynamic)
                return (TMapping)mapping;

            using (var validationContext = new ValidationContext<IMappingClassReference>(_mappingValidatorFactory))
            {
                validationContext.Validate(mapping);
            }

            _mappings.Add(type, mapping);

            return (TMapping)mapping;
        }
    }
}