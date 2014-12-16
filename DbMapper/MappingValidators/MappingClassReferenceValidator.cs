using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IMappingClassReference))]
    public sealed class MappingClassReferenceValidator : MappingValidator
    {
        public MappingClassReferenceValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var mappingClassReference = mapping as IMappingClassReference;
            if (mappingClassReference == null)
                throw new ValidationException("Mapping class reference validation error, mapping '{0}' is not a mapping class reference", mapping.GetType().AssemblyQualifiedName);

            if (mappingClassReference.Type == null)
                throw new ValidationException("Mapping class reference validation error, type is null");            
        }
    }
}