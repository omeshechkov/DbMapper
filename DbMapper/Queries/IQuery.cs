using DbMapper.Statements;

namespace DbMapper.Queries
{
    public interface IQuerySingle<T>
    {
        ISelectStatement Statement { get; }
    }

    public interface IQueryCollection<T>
    {
        IQueryCollection<T, TThrough> Through<TThrough>(ThroughFunc<T, TThrough> through, QueryFunc<TThrough> query);
    }

    public interface IQueryCollection<TTarget, TThrough>
    {
        
    }

    public interface ILoadSingle<TContainer, TTarget>
    {
        
    }

    public interface ILoadSubCollection<TContainer, TTarget>
    {
    }

    public interface IThrough<TContainer, TTarget>
    {
        ILoadSubCollection<TContainer, TTarget, TThrough> Through<TThrough>(ThroughFunc<TContainer, TThrough> through1, ThroughFunc<TTarget, TThrough> through2);
    }

    public interface ILoadSubCollection<TContainer, TTarget, TThrough>
    {
        
    }
}
