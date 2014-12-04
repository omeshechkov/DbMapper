using System;
using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IHasProperties
    {
        IList<IPropertyMapping> Properties { get; }
    }

    public interface IDiscriminatorColumn
    {
        string Column { get; }

        Type Type { get; }
    }

    public interface ITableViewMapping : IMapping, IHasProperties, IHasDiscriminatorColumn, IHasSubClasses { }

    public interface ISubClassMapping : IHasProperties, IHasSubClasses
    {
        Type Type { get; }

        ISubClassJoin Join { get; }
    }

    public interface ISubClassJoin 
    {
        string Table { get; }

        string Schema { get; }

        IList<ISubClassJoinColumn> ColumnJoins { get; }
    }

    public interface ISubClassJoinColumn
    {
        string Name { get; }

        string JoinSchema { get; }

        string JoinTable { get; }

        string JoinColumn { get; }
    }
}