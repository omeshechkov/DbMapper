using System;
using System.Collections.Generic;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    internal sealed class TableMappingValidator : IMappingValidator
    {
        private static readonly IEnumerable<Type> _supportedMappingTypes = new[]
        {
            typeof (ITableMapping)
        };

        public IEnumerable<Type> SupportedMappingTypes
        {
            get { return _supportedMappingTypes; }
        }

        public void Validate(IMappingClassReference mapping)
        {
            if (mapping == null)
                throw new ValidationException("Cannot validate mapping, mapping is null");

            var tableMapping = mapping as ITableMapping;
            if (tableMapping == null)
                throw new ValidationException("Cannot validate mapping, mapping '{0}' is not a table mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(tableMapping.Name))
                throw new ValidationException("Cannot validate table mapping, table is null or empty");

            if (tableMapping.Type == null)
                throw new ValidationException("Cannot validate table mapping, type is null");
        }
    }

    sealed class ExtendTableMappingValidator : IReferenceMappingValidator
    {
        private static readonly IEnumerable<Type> _supportedMappingTypes = new[]
        {
            typeof (ITableMapping),
            typeof (IExtendTableMapping)
        };

        public IEnumerable<Type> SupportedMappingTypes
        {
            get { return _supportedMappingTypes; }
        }

        public void Validate(IMappingClassReference mapping)
        {
            if (mapping == null)
                throw new ValidationException("Cannot validate mapping, mapping is null");

            var tableMapping = mapping as ITableMapping;
            if (tableMapping == null)
                throw new ValidationException("Cannot validate mapping, mapping '{0}' is not a table mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(tableMapping.Name))
                throw new ValidationException("Cannot validate table mapping, table is null or empty");

            if (tableMapping.Type == null)
                throw new ValidationException("Cannot validate table mapping, type is null");
        }

        public void BeginValidate()
        {
            throw new NotImplementedException();
        }

        public void EndValidate()
        {
            throw new NotImplementedException();
        }
    }
}
