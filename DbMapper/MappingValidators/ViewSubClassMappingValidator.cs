using System;
using System.Linq;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IViewSubClassMapping))]
    public sealed class ViewSubClassMappingValidator : MappingValidator
    {
        public ViewSubClassMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("View subclass mapping validation error, mapping is null");

            var subClassMapping = mapping as IViewSubClassMapping;
            if (subClassMapping == null)
                throw new ValidationException("View subclass mapping validation error, mapping '{0}' is not a view subclass mapping", mapping.GetType().AssemblyQualifiedName);

            if (subClassMapping.Type == null)
                throw new ValidationException("View subclass mapping validation error, type is null");

            var parent = subClassMapping.Parent;
            if (parent == null)
                throw new ValidationException("View subclass mapping '{0}' validation error, parent class is null", subClassMapping.Type.AssemblyQualifiedName);

            if (subClassMapping.Properties == null)
                throw new ValidationException("View subclass mapping '{0}' validation error, properties is null", subClassMapping.Type.AssemblyQualifiedName);

            if (!parent.Type.IsAssignableFrom(subClassMapping.Type))
                throw new ValidationException("View subclass mapping '{0}' validation error, class '{0}' is not inherited from '{1}'",
                    subClassMapping.Type.AssemblyQualifiedName, parent.Type);

            try
            {
                var tableMapping = (ITableMapping)context;

                var discriminator = tableMapping.Discriminator;
                if (discriminator != null)
                {
                    if (subClassMapping.Type.IsAbstract)
                    {
                        if (subClassMapping.DiscriminatorValue != null)
                        {
                            throw new ValidationException("View subclass mapping '{0}' validation error, abstract class cannot have discriminator-value",
                                subClassMapping.Type.AssemblyQualifiedName);
                        }
                    }
                    else
                    {
                        if (subClassMapping.DiscriminatorValue == null)
                        {
                            throw new ValidationException("View subclass mapping '{0}' validation error, non abstact class with discriminator column should have discriminator-value",
                                subClassMapping.Type.AssemblyQualifiedName);
                        }

                        if (subClassMapping.DiscriminatorValue.GetType() != discriminator.Type)
                        {
                            throw new ValidationException(
                                "View subclass mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                                subClassMapping.Type.AssemblyQualifiedName, discriminator.Type.AssemblyQualifiedName, subClassMapping.DiscriminatorValue.GetType());
                        }
                    }
                }

                using (var validationContext = new ValidationContext<ISubClassJoin>(Factory))
                {
                    foreach (var propertyMapping in subClassMapping.Properties)
                    {
                        validationContext.Validate(propertyMapping);
                    }
                }

                using (var validationContext = new ValidationContext<IViewPropertyMapping>(Factory))
                {
                    foreach (var propertyMapping in subClassMapping.Properties)
                    {
                        validationContext.Validate(propertyMapping);
                    }
                }

                if (subClassMapping.SubClasses != null && subClassMapping.SubClasses.Any())
                {
                    using (var validationContext = new ValidationContext<ISubClassMapping>(Factory))
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
                throw new ValidationException(string.Format("View subclass mapping '{0}' validation error", subClassMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}