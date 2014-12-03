using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbMapper.Statements
{
    public interface ISelectStatement
    {
        ICollection<IColumnIdentifier> Columns { get; }

        IList<ITableIdentifier> Tables { get; }

        IWhereStatement Where { get; }
    }

    public interface IColumnIdentifier
    {
        ITableIdentifier Table { get; }

        MemberInfo Target { get; set; }

        string Name { get; }
    }

    public interface ITableIdentifier
    {
        string TableName { get; }
        
        Type ClassType { get; set; }
    }

    public interface IWhereStatement
    {
        IExpression Expression { get; }
    }
}