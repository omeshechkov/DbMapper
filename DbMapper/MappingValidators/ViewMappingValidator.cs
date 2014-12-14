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
            //TODO begin: Move to TableOrViewMappingValidator
            if (mapping == null)
                throw new ValidationException("View mapping validation error, mapping is null");

            var viewMapping = mapping as IViewMapping;
            if (viewMapping == null)
                throw new ValidationException("View mapping validation error, mapping '{0}' is not a view mapping", mapping.GetType().AssemblyQualifiedName);

            if (viewMapping.Type == null)
                throw new ValidationException("View mapping validation error, type is null");

            if (string.IsNullOrEmpty(viewMapping.Name))
                throw new ValidationException("View mapping '{0}' validation error, view name is null or empty", viewMapping.Type.AssemblyQualifiedName);

            if (viewMapping.Properties.Count == 0)
                throw new ValidationException("View mapping '{0}' validation error, no properties", viewMapping.Type.AssemblyQualifiedName);

            try
            {
                var discriminator = viewMapping.Discriminator;
                if (discriminator != null)
                {
                    using (var validationContext = new ValidationContext<IDiscriminatorMapping>(Factory))
                    {
                        validationContext.Validate(discriminator);
                    }

                    if (viewMapping.Type.IsAbstract)
                    {
                        if (viewMapping.DiscriminatorValue != null)
                        {
                            throw new ValidationException("View mapping '{0}' validation error, abstract class cannot have discriminator-value",
                                viewMapping.Type.AssemblyQualifiedName);
                        }
                    }
                    else
                    {
                        if (viewMapping.DiscriminatorValue == null)
                        {
                            throw new ValidationException("View mapping '{0}' validation error, non abstact class with discriminator column should have discriminator-value",
                                viewMapping.Type.AssemblyQualifiedName);
                        }

                        if (viewMapping.DiscriminatorValue.GetType() != discriminator.Type)
                        {
                            throw new ValidationException(
                                "View mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                                viewMapping.Type.AssemblyQualifiedName, discriminator.Type.AssemblyQualifiedName, viewMapping.DiscriminatorValue.GetType());
                        }
                    }
                }
                //end TODO

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