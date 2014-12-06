using System;
using System.Xml.Linq;
using DbMapper.Mappings;

namespace DbMapper
{
    public interface IMappingBuilder
    {
        void Configure(XElement configuration);

        void RegisterMapping(IDbMapping mapping);

        TMapping GetMapping<TMapping>(Type type) where TMapping : IDbMapping;
    }
}
