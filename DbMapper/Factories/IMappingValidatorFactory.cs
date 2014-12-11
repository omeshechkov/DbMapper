using System;
using System.Collections.Generic;
using DbMapper.MappingValidators;

namespace DbMapper.Factories
{
    public interface IMappingValidatorFactory
    {
        IDictionary<Type, IList<IMappingValidator>> GetMappingValidators(Type type);

        IDictionary<Type, IList<IMappingValidator>> GetMappingValidators<T>();
    }
}
