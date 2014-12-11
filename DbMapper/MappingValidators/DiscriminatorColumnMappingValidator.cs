using System;
using System.Collections.Generic;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IDiscriminatorColumnMapping))]
    internal class DiscriminatorColumnMappingValidator : MappingValidator
    {
        private static readonly IList<Type> SupportedTypes = new[]
        {
            typeof (bool),
            typeof (string),
            typeof (Type),
            typeof (byte),
            typeof (short),
            typeof (int),
            typeof (long),
            typeof (float),
            typeof (double),
            typeof (decimal),
            typeof (Guid),

            typeof (bool?),
            typeof (byte?),
            typeof (short?),
            typeof (int?),
            typeof (long?),
            typeof (float?),
            typeof (double?),
            typeof (decimal?),
            typeof (Guid?)
        };

        private static readonly string SupportedTypesString = string.Join(", ", SupportedTypes);

        public DiscriminatorColumnMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping)
        {
            if (mapping == null)
                throw new ValidationException("Discriminator mapping validation error, mapping is null");

            var discriminatorColumnMapping = mapping as IDiscriminatorColumnMapping;
            if (discriminatorColumnMapping == null)
                throw new ValidationException("Discriminator mapping validation error, mapping '{0}' is not a discriminator column mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(discriminatorColumnMapping.Column))
                throw new ValidationException("Discriminator mapping validation error, column is null or empty");

            if (discriminatorColumnMapping.Type == null)
                throw new ValidationException("Discriminator mapping validation error, type is null or empty");

            if (!SupportedTypes.Contains(discriminatorColumnMapping.Type))
            {
                throw new ValidationException("Discriminator mapping validation error, type '{0}' is not supported, supported types: [{1}]", discriminatorColumnMapping.Type, SupportedTypesString);
            }
        }
    }
}