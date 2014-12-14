namespace DbMapper.Mappings
{
    public interface ITableViewMapping : IDbMapping, IMutableMapping, IHasProperties, IHasDiscriminator
    {
        object DiscriminatorValue { get; }
    }
}