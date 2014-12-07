using System;
using System.Collections.Generic;

namespace DbMapper
{
    public interface IDbModule
    {
        Type QueryBuilderType { get; }

        IEnumerable<IMappingValidator> Validators { get; }

        IQueryBuilder CreateQueryBuilder();
    }
}
