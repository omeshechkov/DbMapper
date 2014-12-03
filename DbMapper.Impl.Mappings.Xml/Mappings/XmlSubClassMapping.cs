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
        public XmlSubClassMapping(IHasDiscriminatorColumn tableMapping, XNamespace xNamespace, XElement xSubClass)
        {
            Type = xSubClass.Attribute("name").GetValueAsType();

            Properties = new List<IPropertyMapping>();

            foreach (var xProperty in xSubClass.Elements(xNamespace + "property"))
            {
                Properties.Add(new XmlTablePropertyMapping(Type, xProperty));
            }

            XAttribute xDiscriminatorValue;
            if (xSubClass.TryGetAttribute("discriminator-value", out xDiscriminatorValue))
            {
                var discriminator = tableMapping.Discriminator;

                if (discriminator == null)
                    throw new DocumentParseException("Cannot parse subclass discriminator value, unknown discriminator type");

                try
                {
                    DiscriminatorValue = TypeUtils.ParseAs(discriminator.Type, xDiscriminatorValue.Value);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot parse subclass discriminator value '{0}' as '{1}'", xDiscriminatorValue.Value, discriminator.Type), ex);
                }
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubSubClass in xSubClass.Elements(xNamespace + "subclass"))
            {
                SubClasses.Add(new XmlSubClassMapping(tableMapping, xNamespace, xSubSubClass));
            }

            XElement xSubClassJoin;
            if (xSubClass.TryGetElement(xNamespace + "join", out xSubClassJoin))
            {
                Join = new XmlSubClassJoin(xNamespace, xSubClassJoin);
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
        public XmlSubClassJoin(XNamespace xNamespace, XElement xSubClassJoin)
        {
            XAttribute xSchema;
            if (xSubClassJoin.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            Name = xSubClassJoin.Attribute("table").Value;

            ColumnJoins = new List<ISubClassJoinColumn>();
            foreach (var xColumn in xSubClassJoin.Elements(xNamespace + "column"))
            {
                ColumnJoins.Add(new XmlSubClassJoinColumn(xColumn));
            }
        }

        public string Schema { get; private set; }

        public string Name { get; private set; }

        public IList<ISubClassJoinColumn> ColumnJoins { get; private set; }
    }

    sealed class XmlSubClassJoinColumn : ISubClassJoinColumn
    {
        public XmlSubClassJoinColumn(XElement xSubClassJoinColumn)
        {
            Name = xSubClassJoinColumn.Attribute("name").Value;

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

            JoinColumn = xSubClassJoinColumn.Attribute("join-column").Value;
        }

        public string Name { get; private set; }

        public string JoinSchema { get; private set; }

        public string JoinTable { get; private set; }

        public string JoinColumn { get; private set; }
    }
}