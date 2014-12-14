namespace DbMapper.Mappings
{
    public interface IHasDiscriminator
    {
        IDiscriminatorMapping Discriminator { get; }
    }
}