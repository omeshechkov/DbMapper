using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlSubClassJoin : ISubClassJoin
    {
        public XmlSubClassJoin(XNamespace xNamespace, XElement xSubClassJoin)
        {
            XAttribute xSchema;
            if (xSubClassJoin.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xTable;
            if (!xSubClassJoin.TryGetAttribute("table", out xTable))
                throw new DocumentParseException("Cannot find table at subclass join mapping");

            Table = xSubClassJoin.Attribute("table").Value;

            ColumnJoins = new List<ISubClassJoinColumn>();
            foreach (var xColumn in xSubClassJoin.Elements(xNamespace + "column"))
            {
                ColumnJoins.Add(new XmlSubClassJoinColumn(xColumn));
            }
        }

        public string Schema { get; private set; }

        public string Table { get; private set; }

        public IList<ISubClassJoinColumn> ColumnJoins { get; private set; }
    }

    sealed class XmlSubClassJoinColumn : ISubClassJoinColumn
    {
        public XmlSubClassJoinColumn(XElement xSubClassJoinColumn)
        {
            XAttribute xName;
            if (!xSubClassJoinColumn.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at subclass join column mapping");

            Name = xName.Value;

            XAttribute xJoinSchema;
            if (xSubClassJoinColumn.TryGetAttribute("join-schema", out xJoinSchema))
            {
                JoinSchema = xJoinSchema.Value;
            }

            XAttribute xJoinTable;
            if (xSubClassJoinColumn.TryGetAttribute("join-table", out xJoinTable))
            {
                JoinTable = xJoinTable.Value;
            }

            XAttribute xJoinColumn;
            if (!xSubClassJoinColumn.TryGetAttribute("join-column", out xJoinColumn))
                throw new DocumentParseException("Cannot find join-column at subclass join column mapping");

            JoinColumn = xJoinColumn.Value;
        }

        public string Name { get; private set; }

        public string JoinSchema { get; private set; }

        public string JoinTable { get; private set; }

        public string JoinColumn { get; private set; }
    }
}