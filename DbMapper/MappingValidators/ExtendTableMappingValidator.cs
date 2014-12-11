using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableMapping))]
    [CanValidate(typeof(IExtendTableMapping))]
    sealed class ExtendTableMappingValidator : IStatefulMappingValidator
    {
        public void Validate(object mapping)
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