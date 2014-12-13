using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface ITableMapping : ITableViewMapping
    {
        IVersionPropertyMapping Version { get; }

        IList<IPropertyMapping> PrimaryKeyProperties { get; }
    }

    public interface ITableSubClassMapping : ISubClassMapping { }
}