using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IExtendTableMapping))]
    public sealed class ExtendTableMappingValidator : MappingValidator
    {
        public ExtendTableMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var extendTableMapping = mapping as IExtendTableMapping;
            if (extendTableMapping == null)
                throw new ValidationException("Extend table mapping '{0}' validation error, mapping is not an extend-table mapping", mapping.GetType().AssemblyQualifiedName);

            if (extendTableMapping.Type == null)
                throw new ValidationException("Extend table mapping validation error, type is null");

            if (extendTableMapping.SubClasses == null || extendTableMapping.SubClasses.Count == 0)
                throw new ValidationException("Extend table mapping '{0}' validation error, there are no subclasses, you have to specify at least one",
                    extendTableMapping.GetType().AssemblyQualifiedName);

            try
            {
                using (var validationContext = new ValidationContext<ITableSubClassMapping>(Factory))
                {
                    foreach (var subClassMapping in extendTableMapping.SubClasses)
                    {
                        validationContext.Validate(subClassMapping, mapping);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Extend table mapping '{0}' validation error", extendTableMapping.Type.AssemblyQualifiedName), ex);
            }
        }        
    }
}