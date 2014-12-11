using System;

namespace DbMapper
{
    public interface IDbModule
    {
        Type QueryBuilderType { get; }

        IQueryBuilder CreateQueryBuilder();
    }
}
