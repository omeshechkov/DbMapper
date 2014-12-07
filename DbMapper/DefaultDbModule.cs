using System;
using System.Collections.Generic;
using DbMapper.MappingValidators;

namespace DbMapper
{
    public abstract class DefaultDbModule : IDbModule
    {
        private readonly IList<IMappingValidator> _validators = new List<IMappingValidator>();

        protected DefaultDbModule()
        {
            _validators.Add(new TableMappingValidator());
            _validators.Add(new ExtendTableMappingValidator());
        }

        public Type QueryBuilderType { get; protected set; }

        public IEnumerable<IMappingValidator> Validators
        {
            get { return _validators; }
        }

        public abstract IQueryBuilder CreateQueryBuilder();

        protected void RegisterMappingValidator(IMappingValidator mappingValidator)
        {
            _validators.Add(mappingValidator);
        }
    }
}