namespace DbMapper.Mappings
{
    public interface IExtendViewMapping : ITableViewMapping
    {
        IDiscriminatorColumnMapping Discriminator { get; }
    }
}