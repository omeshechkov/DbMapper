using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITablePropertyMapping))]
    public sealed class TablePropertyMappingValidator : MappingValidator
    {

        public TablePropertyMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Table property mapping validation error, mapping is null");

            var tablePropertyMapping = mapping as ITablePropertyMapping;
            if (tablePropertyMapping == null)
                throw new ValidationException("Table property mapping validation error, mapping '{0}' is not a property mapping", mapping.GetType().AssemblyQualifiedName);

            if (tablePropertyMapping.Generator != null)
            {
                using (var validationContext = new ValidationContext<IGenerator>(Factory))
                {
                    validationContext.Validate(tablePropertyMapping.Generator);
                }
            }
        }
    }
}