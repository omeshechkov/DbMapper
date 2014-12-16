using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IExtendTableMapping : IMappingClassReference, IHasDiscriminator
    {
        IList<ISubClassMapping> SubClasses { get; }
    }
}