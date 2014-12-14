using System;
using System.Linq;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableSubClassMapping))]
    public sealed class TableSubClassMappingValidator : MappingValidator
    {
        public TableSubClassMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Table subclass mapping validation error, mapping is null");

            var subClassMapping = mapping as ITableSubClassMapping;
            if (subClassMapping == null)
                throw new ValidationException("Table subclass mapping validation error, mapping '{0}' is not a table subclass mapping", mapping.GetType().AssemblyQualifiedName);

            try
            {
                if (subClassMapping.Properties != null && subClassMapping.Properties.Any())
                {
                    using (var validationContext = new ValidationContext<ITablePropertyMapping>(Factory))
                    {
                        foreach (var propertyMapping in subClassMapping.Properties)
                        {
                            validationContext.Validate(propertyMapping);
                        }
                    }
                }

                if (subClassMapping.SubClasses != null && subClassMapping.SubClasses.Any())
                {
                    using (var validationContext = new ValidationContext<ITableSubClassMapping>(Factory))
                    {
                        foreach (var subSubClassMapping in subClassMapping.SubClasses)
                        {
                            validationContext.Validate(subSubClassMapping, context);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Table subclass mapping '{0}' validation error", subClassMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}