using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ISubClassMapping))]
    public sealed class SubClassMappingValidator : MappingValidator
    {
        public SubClassMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Subclass mapping validation error, mapping is null");

            var subClassMapping = mapping as ISubClassMapping;
            if (subClassMapping == null)
                throw new ValidationException("Subclass mapping validation error, mapping '{0}' is not a subclass mapping", mapping.GetType().AssemblyQualifiedName);

            if (subClassMapping.Type == null)
                throw new ValidationException("Subclass mapping validation error, type is null");

            var parent = subClassMapping.Parent;
            if (parent == null)
                throw new ValidationException("Subclass mapping '{0}' validation error, parent class is null", subClassMapping.Type.AssemblyQualifiedName);

            if (!parent.Type.IsAssignableFrom(subClassMapping.Type))
                throw new ValidationException("Subclass mapping '{0}' validation error, class '{0}' is not inherited from '{1}'",
                    subClassMapping.Type.AssemblyQualifiedName, parent.Type.AssemblyQualifiedName);

            if (context == null)
                throw new ValidationException("Subclass mapping '{0}' validation error, context is null",
                    subClassMapping.Type.AssemblyQualifiedName);

            var discriminatorContainer = context as IHasDiscriminator;

            if (discriminatorContainer == null)
                throw new ValidationException("Subclass mapping '{0}' validation error, context of type '{1}' doesn't contain discriminator column",
                    subClassMapping.Type.AssemblyQualifiedName, context.GetType().AssemblyQualifiedName);

            var discriminator = discriminatorContainer.Discriminator;
            if (discriminator != null)
            {
                if (subClassMapping.Type.IsAbstract)
                {
                    if (subClassMapping.DiscriminatorValue != null)
                    {
                        throw new ValidationException("Subclass mapping '{0}' validation error, abstract class cannot have discriminator-value",
                            subClassMapping.Type.AssemblyQualifiedName);
                    }
                }
                else
                {
                    if (subClassMapping.DiscriminatorValue == null)
                    {
                        throw new ValidationException("Subclass mapping '{0}' validation error, non abstact class with discriminator column should have discriminator-value",
                            subClassMapping.Type.AssemblyQualifiedName);
                    }

                    if (subClassMapping.DiscriminatorValue.GetType() != discriminator.Type)
                    {
                        throw new ValidationException(
                            "Subclass mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                            subClassMapping.Type.AssemblyQualifiedName, discriminator.Type.AssemblyQualifiedName, subClassMapping.DiscriminatorValue.GetType().AssemblyQualifiedName);
                    }
                }
            }
            else if (subClassMapping.Join == null)
            {
                throw new ValidationException("Subclass mapping '{0}' validation error, subclass without discriminator has to have join",
                    subClassMapping.Type.AssemblyQualifiedName);
            }

            try
            {
                if (subClassMapping.Join != null)
                {
                    using (var validationContext = new ValidationContext<ISubClassJoin>(Factory))
                    {
                        validationContext.Validate(subClassMapping.Join);
                    }
                }               
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Subclass mapping '{0}' validation error", subClassMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}