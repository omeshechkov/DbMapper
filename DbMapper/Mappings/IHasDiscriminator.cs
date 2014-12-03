using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IHasDiscriminatorColumn
    {
        IDiscriminatorColumn Discriminator { get; }        
    }

    public interface IHasSubClasses
    {
        IList<ISubClassMapping> SubClasses { get; }

        object DiscriminatorValue { get; }
    }
}