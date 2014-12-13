using System;
using System.Linq;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableMapping))]
    internal sealed class TableMappingValidator : MappingValidator
    {
        public TableMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Table mapping validation error, mapping is null");

            var tableMapping = mapping as ITableMapping;
            if (tableMapping == null)
                throw new ValidationException("Table mapping validation error, mapping '{0}' is not a table mapping", mapping.GetType().AssemblyQualifiedName);

            if (tableMapping.Type == null)
                throw new ValidationException("Table mapping validation error, type is null");

            if (string.IsNullOrEmpty(tableMapping.Name))
                throw new ValidationException("Table mapping '{0}' validation error, table name is null or empty", tableMapping.Type.AssemblyQualifiedName);

            if (tableMapping.Properties.Count == 0)
                throw new ValidationException("Table mapping '{0}' validation error, no properties", tableMapping.Type.AssemblyQualifiedName);

            try
            {
                var discriminator = tableMapping.Discriminator;
                if (discriminator != null)
                {
                    using (var validationContext = new ValidationContext<IDiscriminatorMapping>(Factory))
                    {
                        validationContext.Validate(discriminator);
                    }

                    if (tableMapping.Type.IsAbstract)
                    {
                        if (tableMapping.DiscriminatorValue != null)
                        {
                            throw new ValidationException("Table mapping '{0}' validation error, abstract class cannot have discriminator-value",
                                tableMapping.Type.AssemblyQualifiedName);
                        }
                    }
                    else
                    {
                        if (tableMapping.DiscriminatorValue == null)
                        {
                            throw new ValidationException("Table mapping '{0}' validation error, non abstact class with discriminator column should have discriminator-value",
                                tableMapping.Type.AssemblyQualifiedName);
                        }

                        if (tableMapping.DiscriminatorValue.GetType() != discriminator.Type)
                        {
                            throw new ValidationException(
                                "Table mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                                tableMapping.Type.AssemblyQualifiedName, discriminator.Type.AssemblyQualifiedName, tableMapping.DiscriminatorValue.GetType());
                        }
                    }
                }

                using (var validationContext = new ValidationContext<ITablePropertyMapping>(Factory))
                {
                    foreach (var propertyMapping in tableMapping.Properties)
                    {
                        validationContext.Validate(propertyMapping);
                    }
                }

                if (tableMapping.SubClasses != null && tableMapping.SubClasses.Any())
                {
                    using (var validationContext = new ValidationContext<ITableSubClassMapping>(Factory))
                    {
                        foreach (var subClassMapping in tableMapping.SubClasses)
                        {
                            validationContext.Validate(subClassMapping, mapping);
                        }
                    }
                }

                if (tableMapping.Version != null)
                {
                    using (var validationContext = new ValidationContext<IVersionPropertyMapping>(Factory))
                    {
                        validationContext.Validate(tableMapping.Version);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format("Table mapping '{0}' validation error", tableMapping.Type.AssemblyQualifiedName), ex);
            }
        }
    }
}