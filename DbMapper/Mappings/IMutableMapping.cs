using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IMutableMapping
    {
        IList<ISubClassMapping> SubClasses { get; }
    }
},