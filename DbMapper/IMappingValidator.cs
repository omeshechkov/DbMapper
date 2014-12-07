using System;
using System.Collections.Generic;
using DbMapper.Mappings;

namespace DbMapper
{
    public interface IMappingValidator
    {
        IEnumerable<Type> SupportedMappingTypes { get; }

        void Validate(IMappingClassReference mapping);
    }

    public interface IReferenceMappingValidator : IMappingValidator
    {
        void BeginValidate();
        
        void EndValidate();
    }
}