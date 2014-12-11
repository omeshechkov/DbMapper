using System;
using System.Collections.Generic;
using System.Reflection;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IPropertyMapping))]
    internal class PropertyMappingValidator : MappingValidator
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
            typeof (DateTime),

            typeof (bool?),
            typeof (byte?),
            typeof (short?),
            typeof (int?),
            typeof (long?),
            typeof (float?),
            typeof (double?),
            typeof (decimal?),
            typeof (Guid?),
            typeof (DateTime)
        };

        private static readonly string SupportedTypesString = string.Join(", ", SupportedTypes);

        public PropertyMappingValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping)
        {
            if (mapping == null)
                throw new ValidationException("Property mapping validation error, mapping is null");

            var propertyMapping = mapping as IPropertyMapping;
            if (propertyMapping == null)
                throw new ValidationException("Property mapping validation error, mapping '{0}' is not a property mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(propertyMapping.Name))
                throw new ValidationException("Property mapping validation error, column is null or empty");

            var memberInfo = propertyMapping.Member;

            if (memberInfo == null)
                throw new ValidationException("Property '{0}' mapping validation error, member is null", propertyMapping.Name);

            if (memberInfo.MemberType != MemberTypes.Field && memberInfo.MemberType != MemberTypes.Property)
                throw new ValidationException("Property mapping validation error, member is not a property or a field");

            Type memberType;
            bool isStatic;

            var fieldInfo = memberInfo as FieldInfo;
            var propertyInfo = memberInfo as PropertyInfo;
            if (fieldInfo != null)
            {
                isStatic = fieldInfo.IsStatic;
                memberType = fieldInfo.FieldType;
            }
            else if (propertyInfo != null)
            {
                if (propertyInfo.CanRead)
                    throw new ValidationException("Property '{0}' mapping validation error, no getter", propertyMapping.Name);

                if (propertyInfo.CanWrite)
                    throw new ValidationException("Property '{0}' mapping validation error, no setter", propertyMapping.Name);

                isStatic = propertyInfo.GetMethod.IsStatic || propertyInfo.SetMethod.IsStatic;
                memberType = propertyInfo.PropertyType;
            }
            else
                throw new ValidationException("Property '{0}' mapping validation error, member is not a property or a field", propertyMapping.Name);

            if (isStatic)
                throw new ValidationException("Property '{0}' mapping validation error, property is static", propertyMapping.Name);

            if (!SupportedTypes.Contains(memberType))
            {
                throw new ValidationException("Property '{0}' mapping validation error, type '{0}' is not supported, supported types: [{1}]",
                    propertyMapping.Name, memberType, SupportedTypesString);
            }
        }
    }
}