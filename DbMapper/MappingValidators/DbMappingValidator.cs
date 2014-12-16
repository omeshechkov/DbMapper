using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IDbMapping))]
    public sealed class DbMappingValidator : MappingValidator
    {
        public DbMappingValidator(IMappingValidatorFactory factory) : base(factory) { }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            var dbMapping = mapping as IDbMapping;
            if (dbMapping == null)
                throw new ValidationException("DB-mapping validation error, mapping '{0}' is not a db-mapping", mapping.GetType().AssemblyQualifiedName);

            if (string.IsNullOrEmpty(dbMapping.Name))
                throw new ValidationException("DB-mapping validation error, name is null or empty");            
        }
    }
}