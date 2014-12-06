namespace DbMapper.Mappings
{
    public interface IExtendTableMapping : IMutableMapping
    {
        IDiscriminatorColumnMapping Discriminator { get; }
    }
}