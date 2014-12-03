using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface ITableMapping : ITableViewMapping, IHasVersion, IHasPrimaryKey { }

    public interface ITablePropertyMapping : IPropertyMapping, IHasGenerator
    {
        bool Insert { get; }
        
        bool Update { get; }
    }

    public interface IHasPrimaryKey
    {
        IList<IPropertyMapping> PrimaryKeyProperties { get; }
    }

    public interface IHasVersion
    {
        IVersionProperty VersionProperty { get; }
    }

    public interface IVersionProperty : IPropertyMapping { }
}