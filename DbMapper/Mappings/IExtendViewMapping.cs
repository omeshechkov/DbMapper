using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IExtendViewMapping : IMappingClassReference, IHasDiscriminator
    {
        IList<ISubClassMapping> SubClasses { get; }
    }
}