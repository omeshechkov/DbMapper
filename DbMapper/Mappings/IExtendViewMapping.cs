namespace DbMapper.Mappings
{
    public interface IExtendViewMapping : IMutableMapping
    {
        IDiscriminatorColumnMapping Discriminator { get; }
    }
}