using DbMapper.Statements;

namespace DbMapper.Queries
{
    public class QuerySingle<T> : IQuerySingle<T>
    {
        public ISelectStatement Statement { get; private set; }

        public QuerySingle()
        {
            Statement = new SelectStatement();
        }
    }
}
