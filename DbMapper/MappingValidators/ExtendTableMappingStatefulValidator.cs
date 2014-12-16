using System;
using System.Collections.Generic;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableMapping))]
    [CanValidate(typeof(IExtendTableMapping))]
    public sealed class ExtendTableMappingStatefulValidator : MappingValidator, IStatefulMappingValidator
    {
        private IList<Type> _registeredTypes;
        private IList<Type> _extendTypes;

        public ExtendTableMappingStatefulValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var tableMapping = mapping as ITableMapping;
            var extendTableMapping = mapping as IExtendTableMapping;

            if (tableMapping == null && extendTableMapping == null)
                throw new ValidationException("Extend table or table mapping validation error, mapping '{0}' is not a table or extend-table mapping", mapping.GetType().AssemblyQualifiedName);

            if (tableMapping != null)
            {
                if (_registeredTypes.Contains(tableMapping.Type))
                    throw new ValidationException("Cannot register table mapping, duplicate mapping for '{0}' type", tableMapping.Type.AssemblyQualifiedName);

                _registeredTypes.Add(tableMapping.Type);
                RegisterMapping(tableMapping);
            }

            if (extendTableMapping != null)
            {
                _extendTypes.Add(extendTableMapping.Type);
                foreach (var subclassMapping in extendTableMapping.SubClasses)
                {
                    RegisterMapping(subclassMapping);
                }                
            } 
        }

        public void BeginValidate()
        {
            _registeredTypes = new List<Type>();
            _extendTypes = new List<Type>();
        }

        public void EndValidate()
        {
            foreach (var extendType in _extendTypes)
            {
                //Extend table type is not assigned from table, table subclass or extend-table subclass type
                if (!_registeredTypes.Contains(extendType))
                    throw new ValidationException("Extend table mapping validation error, there is no table/subclass mapping for '{0}'", extendType.AssemblyQualifiedName);
            }
        }

        private void RegisterMapping(IMutableMapping mapping)
        {
            foreach (var subClassMapping in mapping.SubClasses)
            {
                if (_registeredTypes.Contains(subClassMapping.Type))
                    throw new ValidationException("Cannot register subclass mapping, duplicate mapping for '{0}' type", subClassMapping.Type.AssemblyQualifiedName);

                _registeredTypes.Add(subClassMapping.Type);

                RegisterMapping(subClassMapping);
            }
        }
    }
}