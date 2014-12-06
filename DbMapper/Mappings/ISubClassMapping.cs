using System;
using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface ISubClassMapping : IMutableMapping, IHasProperties
    {
        object DiscriminatorValue { get; }

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