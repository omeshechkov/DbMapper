using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ISubClassJoinColumn))]
    public sealed class SubClassJoinColumnValidator : MappingValidator
    {
        public SubClassJoinColumnValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var joinColumn = mapping as ISubClassJoinColumn;
            if (joinColumn == null)
                throw new ValidationException("Subclass join column mapping validation error, mapping '{0}' is not a subclass column join mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(joinColumn.Name))
                throw new ValidationException("Subclass join column mapping validation error, name is null or empty");

            if (string.IsNullOrEmpty(joinColumn.JoinColumn))
                throw new ValidationException("Subclass join column mapping validation error, join column is null or empty");
        }
    }
}