using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ITableViewMapping))]
    public sealed class TableViewMappingValidator : MappingValidator
    {
        public TableViewMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var tableViewMapping = mapping as ITableViewMapping;
            if (tableViewMapping == null)
                throw new ValidationException("Table/view mapping validation error, mapping '{0}' is not a table or view mapping", mapping.GetType().AssemblyQualifiedName);

            if (tableViewMapping.Properties == null || tableViewMapping.Properties.Count == 0)
                throw new ValidationException("Table/view mapping '{0}' validation error, no properties", tableViewMapping.Type.AssemblyQualifiedName);
        }
    }
}