using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IMutableMapping))]
    public sealed class MutableMappingValidator : MappingValidator
    {
        public MutableMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var mutableMapping = mapping as IMutableMapping;
            if (mutableMapping == null)
                throw new ValidationException("Mutable mapping validation error, mapping '{0}' is not a mutable mapping", mapping.GetType().AssemblyQualifiedName);

            if (context == null)
                throw new ValidationException("Mutable mapping '{0}' validation error, context is null",
                    mutableMapping.Type.AssemblyQualifiedName);

            var discriminatorContainer = context as IHasDiscriminator;

            if (discriminatorContainer == null)
                throw new ValidationException("Mutable mapping '{0}' validation error, context of type '{1}' doesn't contain discriminator column",
                    mutableMapping.Type.AssemblyQualifiedName, context.GetType().AssemblyQualifiedName);

            var discriminator = discriminatorContainer.Discriminator;
            if (discriminator != null)
            {
                if (mutableMapping.Type.IsAbstract)
                {
                    if (mutableMapping.DiscriminatorValue != null)
                    {
                        throw new ValidationException("Mutable mapping '{0}' validation error, abstract class cannot have discriminator-value",
                            mutableMapping.Type.AssemblyQualifiedName);
                    }
                }
                else
                {
                    if (mutableMapping.DiscriminatorValue == null)
                    {
                        throw new ValidationException("Mutable mapping '{0}' validation error, non abstact class with discriminator column has to have discriminator-value",
                            mutableMapping.Type.AssemblyQualifiedName);
                    }

                    if (mutableMapping.DiscriminatorValue.GetType() != discriminator.Type)
                    {
                        throw new ValidationException(
                            "Mutable mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                            mutableMapping.Type.AssemblyQualifiedName, discriminator.Type.AssemblyQualifiedName, mutableMapping.DiscriminatorValue.GetType().AssemblyQualifiedName);
                    }
                }
            }
        }
    }
}