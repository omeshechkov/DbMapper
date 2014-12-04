using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlSubClassMapping : ISubClassMapping
    {
        public XmlSubClassMapping(IDiscriminatorColumn discriminatorColumn, XElement xSubClass)
        {
            XAttribute xName;
            if (!xSubClass.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at table subclass mapping");

            try
            {
                Type = xName.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize subclass '{0}' at table subclass mapping", xName.Value));
            }

            XElement xSubClassJoin;
            if (xSubClass.TryGetElement(XmlTableMapping.XNamespace + "join", out xSubClassJoin))
            {
                Join = new XmlSubClassJoin(xSubClassJoin);
            }

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xSubClass.Elements(XmlTableMapping.XNamespace + "property"))
            {
                Properties.Add(new XmlTablePropertyMapping(Type, xProperty));
            }

            XAttribute xDiscriminatorValue;
            if (xSubClass.TryGetAttribute("discriminator-value", out xDiscriminatorValue))
            {
                if (discriminatorColumn == null)
                    throw new DocumentParseException("Cannot parse subclass discriminator value at table subclass mapping, unknown discriminator type");

                try
                {
                    DiscriminatorValue = TypeUtils.ParseAs(discriminatorColumn.Type, xDiscriminatorValue.Value);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot parse subclass discriminator value '{0}' as '{1}' at table subclass mapping", xDiscriminatorValue.Value, discriminatorColumn.Type.AssemblyQualifiedName), ex);
                }
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubSubClass in xSubClass.Elements(XmlTableMapping.XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlSubClassMapping(discriminatorColumn, xSubSubClass));
            }
        }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }

        public ISubClassJoin Join { get; private set; }
    }

    sealed class XmlSubClassJoin : ISubClassJoin
    {
        public XmlSubClassJoin(XElement xSubClassJoin)
        {
            XAttribute xSchema;
            if (xSubClassJoin.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xTable;
            if (!xSubClassJoin.TryGetAttribute("table", out xTable))
                throw new DocumentParseException("Cannot find table at table subclass join mapping");

            Table = xSubClassJoin.Attribute("table").Value;

            ColumnJoins = new List<ISubClassJoinColumn>();
            foreach (var xColumn in xSubClassJoin.Elements(XmlTableMapping.XNamespace + "column"))
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
                throw new DocumentParseException("Cannot find name at table subclass join column mapping");

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
                throw new DocumentParseException("Cannot find join-column at table subclass join column mapping");

            JoinColumn = xJoinColumn.Value;
        }

        public string Name { get; private set; }

        public string JoinSchema { get; private set; }

        public string JoinTable { get; private set; }

        public string JoinColumn { get; private set; }
    }
}