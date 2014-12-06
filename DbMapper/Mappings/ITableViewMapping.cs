namespace DbMapper.Mappings
{
    public interface ITableViewMapping : IMapping, IMutableMapping, IHasProperties
    {
        object DiscriminatorValue { get; }
    }
}