using System;
using System.Collections.Generic;
using DbMapper.Factories;
using DbMapper.MappingValidators;

namespace DbMapper.Test.MappingValidators
{
    class MappingValidatorFactoryStub : IMappingValidatorFactory
    {
        public IDictionary<Type, IList<IMappingValidator>> GetMappingValidators(Type type)
        {
            return new Dictionary<Type, IList<IMappingValidator>>();
        }

        public IDictionary<Type, IList<IMappingValidator>> GetMappingValidators<T>()
        {
            return new Dictionary<Type, IList<IMappingValidator>>();
        }
    }
}
