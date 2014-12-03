using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbMapper.Statements
{
    public class SelectStatement : ISelectStatement
    {
        public ICollection<IColumnIdentifier> Columns { get; private set; }

        public IList<ITableIdentifier> Tables { get; private set; }

        public IWhereStatement Where { get; private set; }
    }

    public class TableIdentifier : ITableIdentifier
    {
        public string TableName { get; internal set; }

        public Type ClassType { get; set; }
    }

    public class ColumnIdentifier : IColumnIdentifier
    {
        public ITableIdentifier Table { get; private set; }

        public MemberInfo Target { get; set; }

        public string Name { get; private set; }
    }

    public class WhereStatement : IWhereStatement
    {
        public IExpression Expression { get; private set; }
    }
}
