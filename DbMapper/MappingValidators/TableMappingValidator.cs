using System;
using System.Linq;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableMapping))]
    public sealed class TableMappingValidator : MappingValidator
    {
        public TableMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var tableMapping = mapping as ITableMapping;
            if (tableMapping == null)
                throw new ValidationException("Table mapping validation error, mapping '{0}' is not a table mapping", mapping.GetType().AssemblyQualifiedName);

            try
            {
                using (var validationContext = new ValidationContext<ITablePropertyMapping>(Factory))
                {
                    foreach (var propertyMapping in tableMapping.Properties)
                    {
                        validationContext.Validate(propertyMapping);
                    }
                }

                if (tableMapping.SubClasses != null && tableMapping.SubClasses.Any())
                {
                    using (var validationContext = new ValidationContext<ITableSubClassMapping>(Factory))
                    {
                        foreach (var subClassMapping in tableMapping.SubClasses)
                        {
                            validationContext.Validate(subClassMapping, mapping);
                        }
                    }
                }

                if (tableMapping.Version != null)
                {
                    using (var validationContext = new ValidationContext<IVersionPropertyMapping>(Factory))
                    {
                        validationContext.Validate(tableMapping.Version);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Table mapping '{0}' validation error", tableMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}