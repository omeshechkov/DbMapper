using System;
using System.Collections.Generic;
using System.Reflection;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IVersionPropertyMapping))]
    public sealed class VersionPropertyMappingValidator : MappingValidator
    {
        private static readonly IList<Type> SupportedTypes = new[]
        {
            typeof (byte),
            typeof (short),
            typeof (int),
            typeof (long),
            typeof (float),
            typeof (double),
            typeof (decimal)
        };

        private static readonly string SupportedTypesString = string.Join(", ", SupportedTypes);

        public VersionPropertyMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Version property mapping validation error, mapping is null");

            var versionPropertyMapping = mapping as IVersionPropertyMapping;
            if (versionPropertyMapping == null)
                throw new ValidationException("Version property mapping validation error, mapping '{0}' is not a property mapping", mapping.GetType().AssemblyQualifiedName);

            var memberInfo = versionPropertyMapping.Member;

            if (memberInfo == null)
                throw new ValidationException("Version property mapping validation error, member is null");

            Type memberType;

            var fieldInfo = memberInfo as FieldInfo;
            var propertyInfo = memberInfo as PropertyInfo;
            if (fieldInfo != null)
            {
                memberType = fieldInfo.FieldType;
            }
            else if (propertyInfo != null)
            {
                memberType = propertyInfo.PropertyType;
            }
            else
                throw new ValidationException("Version property mapping validation error, member is not a property or a field");

            if (!SupportedTypes.Contains(memberType))
            {
                throw new ValidationException("Version property mapping validation error, type '{0}' is not supported, supported types: [{1}]",
                    memberType.AssemblyQualifiedName, SupportedTypesString);
            }
        }
    }
}