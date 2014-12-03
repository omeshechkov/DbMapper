using System;
using System.Collections.Generic;
using DbMapper.Queries;

namespace DbMapper.QueryBuilder
{
    public class QueryBuilder : IQueryBuilder
    {
        public IQuerySingle<T> Get<T>(QueryFunc<T> query, Func<IList<T>> columns = null, bool throwIfNotExists = true)
        {
            return new QuerySingle<T>();
        }

        public IQueryCollection<T> Collect<T>(QueryFunc<T> query = null, ColumnsFunc<T> columns = null)
        {
            throw new NotImplementedException();
        }

        public ICollectionQueryBuilder<T> ForEach<T>()
        {
            return new CollectionQueryBuilder<T>();
        }
    }

    public class CollectionQueryBuilder<T> : ICollectionQueryBuilder<T>
    {
        public Type Type { get; private set; }

        public CollectionQueryBuilder()
        {
            Type = typeof(T);
        }

        public ILoadSingle<T, TTarget> Load<TTarget>(SingleTarget<T, TTarget> target, Join<T, TTarget> join, ColumnsFunc<TTarget> columns = null, bool throwIfNotExists = true)
        {
            throw new NotImplementedException();
        }

        public ILoadSubCollection<T, TTarget> Load<TTarget>(CollectionTarget<T, TTarget> target, Join<T, TTarget> join, ColumnsFunc<TTarget> columns = null)
        {
            throw new NotImplementedException();
        }

        public IThrough<T, TTarget> Load<TTarget>(CollectionTarget<T, TTarget> target, QueryFunc<TTarget> query, ColumnsFunc<TTarget> columns = null)
        {
            throw new NotImplementedException();
        }

        public IThrough<T, TTarget> Load<TTarget>(CollectionTarget<T, TTarget> target, ColumnsFunc<TTarget> columns = null)
        {
            throw new NotImplementedException();
        }
    }
}
