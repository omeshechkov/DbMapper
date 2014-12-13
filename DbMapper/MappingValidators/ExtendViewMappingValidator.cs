using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IExtendViewMapping))]
    public sealed class ExtendViewMappingValidator : MappingValidator
    {
        public ExtendViewMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Extend view mapping validation error, mapping is null");

            var extendViewMapping = mapping as IExtendViewMapping;
            if (extendViewMapping == null)
                throw new ValidationException("Extend view mapping '{0}' validation error, mapping is not an extend-view mapping", mapping.GetType().AssemblyQualifiedName);

            if (extendViewMapping.Type == null)
                throw new ValidationException("Extend view mapping validation error, type is null");

            if (extendViewMapping.SubClasses == null || extendViewMapping.SubClasses.Count == 0)
                throw new ValidationException("Extend view mapping '{0}' validation error, there are no subclasses, you have to specify at least one",
                    extendViewMapping.GetType().AssemblyQualifiedName);

            try
            {
                using (var validationContext = new ValidationContext<IViewSubClassMapping>(Factory))
                {
                    foreach (var subClassMapping in extendViewMapping.SubClasses)
                    {
                        validationContext.Validate(subClassMapping, mapping);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Extend view mapping '{0}' validation error", extendViewMapping.Type.AssemblyQualifiedName), ex);
            }
        }     
    }
}