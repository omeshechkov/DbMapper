using System;
using System.Linq;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IViewMapping))]
    public sealed class ViewMappingValidator : MappingValidator
    {
        public ViewMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var viewMapping = mapping as IViewMapping;
            if (viewMapping == null)
                throw new ValidationException("View mapping validation error, mapping '{0}' is not a view mapping", mapping.GetType().AssemblyQualifiedName);

            try
            {
                using (var validationContext = new ValidationContext<IViewPropertyMapping>(Factory))
                {
                    foreach (var propertyMapping in viewMapping.Properties)
                    {
                        validationContext.Validate(propertyMapping);
                    }
                }

                if (viewMapping.SubClasses != null && viewMapping.SubClasses.Any())
                {
                    using (var validationContext = new ValidationContext<IViewSubClassMapping>(Factory))
                    {
                        foreach (var subClassMapping in viewMapping.SubClasses)
                        {
                            validationContext.Validate(subClassMapping, mapping);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("View mapping '{0}' validation error", viewMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}