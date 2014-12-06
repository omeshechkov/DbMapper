using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface ITableMapping : ITableViewMapping
    {
        IDiscriminatorColumnMapping Discriminator { get; }

        IVersionPropertyMapping Version { get; }

        IList<IPropertyMapping> PrimaryKeyProperties { get; }
    }
}