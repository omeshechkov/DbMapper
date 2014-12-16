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
                throw new ArgumentNullException("mapping");

            var subClassMapping = mapping as ISubClassMapping;
            if (subClassMapping == null)
                throw new ValidationException("Subclass mapping validation error, mapping '{0}' is not a subclass mapping", mapping.GetType().AssemblyQualifiedName);

            var parent = subClassMapping.Parent;
            if (parent == null)
                throw new ValidationException("Subclass mapping '{0}' validation error, parent class is null", subClassMapping.Type.AssemblyQualifiedName);

            if (!parent.Type.IsAssignableFrom(subClassMapping.Type))
                throw new ValidationException("Subclass mapping '{0}' validation error, class '{0}' is not inherited from '{1}'",
                    subClassMapping.Type.AssemblyQualifiedName, parent.Type.AssemblyQualifiedName);

            if (subClassMapping.DiscriminatorValue == null && subClassMapping.Join == null)
            {
                throw new ValidationException("Subclass mapping '{0}' validation error, subclass has to have discriminator or join at least",
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