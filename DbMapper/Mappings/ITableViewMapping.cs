namespace DbMapper.Mappings
{
    public interface ITableViewMapping : IDbMapping, IMutableMapping, IHasProperties
    {
        IDiscriminatorColumnMapping Discriminator { get; }

        object DiscriminatorValue { get; }
    }
}