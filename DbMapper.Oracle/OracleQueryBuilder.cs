using System;
using System.Collections.Generic;
using DbMapper.Queries;

namespace DbMapper.Oracle
{
    public class OracleQueryBuilder : IOracleQueryBuilder
    {
        public IQuerySingle<T> Get<T>(QueryFunc<T> query, Func<IList<T>> columns = null, bool throwIfNotExists = true)
        {
            throw new NotImplementedException();
        }

        public IQueryCollection<T> Collect<T>(QueryFunc<T> query = null, ColumnsFunc<T> columns = null)
        {
            throw new NotImplementedException();
        }

        public ICollectionQueryBuilder<T> ForEach<T>()
        {
            throw new NotImplementedException();
        }

        public IFunction<TFunction> Function<TFunction>()
        {
            throw new NotImplementedException();
        }

        public IProcedure<TProcedure> Procedure<TProcedure>()
        {
            throw new NotImplementedException();
        }
    }
}
