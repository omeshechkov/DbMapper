using System;
using System.Collections.Generic;
using System.Data;
using DbMapper.Queries;

namespace DbMapper
{
    public class Connection : IConnection
    {
        public IDbConnection NativeConnection { get; private set; }
        
        public IDialect Dialect { get; private set; }

        public T Get<T>(IQuerySingle<T> query)
        {
            throw new NotImplementedException();
        }

        public IList<T> Get<T>(IQueryCollection<T> query)
        {
            throw new NotImplementedException();
        }

        public IList<TTarget> Get<TTarget, TThrough>(IQueryCollection<TTarget, TThrough> query)
        {
            throw new NotImplementedException();
        }

        public void ForEach<TContainer, TTarget>(IList<TContainer> items, ILoadSingle<TContainer, TTarget> query)
        {
            throw new NotImplementedException();
        }

        public void ForEach<TContainer, TTarget>(IList<TContainer> items, ILoadSubCollection<TContainer, TTarget> query)
        {
            throw new NotImplementedException();
        }

        public void ForEach<TContainer, TTarget, TThrough>(IList<TContainer> items, ILoadSubCollection<TContainer, TTarget, TThrough> query)
        {
            throw new NotImplementedException();
        }
    }
}
