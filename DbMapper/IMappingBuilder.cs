using System;
using System.Xml.Linq;
using DbMapper.Mappings;

namespace DbMapper
{
    public interface IMappingBuilder
    {
        void Configure(XElement configuration);

        void RegisterMapping(IMapping mapping);

        TMapping GetMapping<TMapping>(Type type) where TMapping : IMapping;
    }
}
