using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IHasProperties
    {
        IList<IPropertyMapping> Properties { get; }
    }
}