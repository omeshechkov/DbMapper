using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Mappings;

namespace DbMapper
{
    public interface IMappingBuilder
    {
        void Configure(XElement configuration);
    }

    public interface IStaticMappingBuilder : IMappingBuilder
    {
        ICollection<IMappingClassReference> Mappings { get; }
    }

    public interface IDynamicMappingBuilder : IMappingBuilder
    {
        bool TryGetMapping<TMapping>(Type type, out TMapping mapping) where TMapping : IMappingClassReference;
    }
}