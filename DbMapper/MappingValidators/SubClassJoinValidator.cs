using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Utils;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(ISubClassJoin))]
    public sealed class SubClassJoinValidator : MappingValidator
    {
        public SubClassJoinValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Subclass join validation error, mapping is null");

            var subClassJoin = mapping as ISubClassJoin;
            if (subClassJoin == null)
                throw new ValidationException("Subclass join validation error, mapping '{0}' is not a subclass join mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(subClassJoin.Table))
                throw new ValidationException("Subclass join validation error, table name is null or empty");

            if (subClassJoin.ColumnJoins == null)
                throw new ValidationException("Subclass join validation error, columns is null");

            if (subClassJoin.ColumnJoins.Count == 0)
                throw new ValidationException("Subclass join validation error, no columns");

            using (var validationContext = new ValidationContext<ISubClassJoinColumn>(Factory))
            {
                foreach (var joinColumn in subClassJoin.ColumnJoins)
                {
                    validationContext.Validate(joinColumn, context);
                }
            }
        }
    }
}