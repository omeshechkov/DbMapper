namespace DbMapper
{
    public interface IDialect
    {
        IQueryBuilder QueryBuilder { get; }
    }
}