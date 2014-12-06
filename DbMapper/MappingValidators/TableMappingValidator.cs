using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    sealed class TableMappingValidator : IMappingValidator<ITableMapping>
    {
        public void Validate(ITableMapping mapping)
        {
            if (string.IsNullOrEmpty(mapping.Name))
                throw new ValidationException("Cannot validate table mapping, table is null or empty");

            if (mapping.Type == null)
                throw new ValidationException("Cannot validate table mapping, type is null");
        }
    }
}
