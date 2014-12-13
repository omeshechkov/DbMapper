namespace DbMapper.Mappings
{
    public interface ITableViewMapping : IDbMapping, IMutableMapping, IHasProperties
    {
        IDiscriminatorMapping Discriminator { get; }

        object DiscriminatorValue { get; }
    }
}