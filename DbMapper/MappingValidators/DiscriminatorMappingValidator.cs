using System;
using System.Collections.Generic;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IDiscriminatorMapping))]
    public sealed class DiscriminatorMappingValidator : MappingValidator
    {
        private static readonly IList<Type> SupportedTypes = new[]
        {
            typeof (string),
            typeof (Type),
            typeof (byte),
            typeof (short),
            typeof (int),
            typeof (long),
            typeof (float),
            typeof (double),
            typeof (decimal),
            typeof (Guid)
        };

        private static readonly string SupportedTypesString = string.Join(", ", SupportedTypes);

        public DiscriminatorMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var discriminatorColumnMapping = mapping as IDiscriminatorMapping;
            if (discriminatorColumnMapping == null)
                throw new ValidationException("Discriminator mapping validation error, mapping '{0}' is not a discriminator column mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(discriminatorColumnMapping.Column))
                throw new ValidationException("Discriminator mapping validation error, column is null or empty");

            if (discriminatorColumnMapping.Type == null)
                throw new ValidationException("Discriminator mapping validation error, type is null");

            if (!SupportedTypes.Contains(discriminatorColumnMapping.Type))
            {
                throw new ValidationException("Discriminator mapping validation error, type '{0}' is not supported, supported types: [{1}]", 
                    discriminatorColumnMapping.Type.AssemblyQualifiedName, SupportedTypesString);
            }
        }
    }
}