using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IMutableMapping : IMappingClassReference
    {
        IList<ISubClassMapping> SubClasses { get; }
    }
}