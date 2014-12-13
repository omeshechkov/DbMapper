using System;
using System.Xml.Linq;

namespace DbMapper
{
    public interface IDbModule
    {
        IMappingProvider MappingProvider { get; }        

        Type QueryBuilderType { get; }

        IQueryBuilder CreateQueryBuilder();

        void Configure(XElement xModule);
    }
}
