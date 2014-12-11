using System;
using DbMapper.Factories;
using DbMapper.MappingValidators;

namespace DbMapper
{
    public abstract class DefaultDbModule : IDbModule
    {
        private readonly MappingValidatorFactory _mappingValidatorFactory = new MappingValidatorFactory();
        protected IMappingValidatorFactory MappingValidatorFactory
        {
            get { return _mappingValidatorFactory; }
        }

        protected DefaultDbModule()
        {
            _mappingValidatorFactory.RegisterValidator(f => new TableMappingValidator(f));
            _mappingValidatorFactory.RegisterValidator(f => new DiscriminatorColumnMappingValidator(f));
            _mappingValidatorFactory.RegisterValidator(f => new PropertyMappingValidator(f));
            _mappingValidatorFactory.RegisterValidator(f => new TablePropertyMappingValidator(f));
            _mappingValidatorFactory.RegisterValidator(f => new VersionPropertyMappingValidator(f));

            _mappingValidatorFactory.RegisterValidator(f => new ExtendTableMappingValidator());
        }

        public Type QueryBuilderType { get; protected set; }

        public abstract IQueryBuilder CreateQueryBuilder();

        protected void RegisterMappingValidator<T>(MappingValidatorCreator<T> creator) where T : IMappingValidator
        {
            _mappingValidatorFactory.RegisterValidator<T>(creator);
        }
    }
}