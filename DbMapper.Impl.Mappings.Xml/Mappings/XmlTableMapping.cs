using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlTableMapping : ITableMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-table-mapping";

        public XmlTableMapping(XContainer xMapping)
        {
            var xTable = xMapping.Element(XNamespace + "table");

            XAttribute xSchema;
            if (xTable.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            Name = xTable.Attribute("name").Value;

            Type = xTable.Attribute("class").GetValueAsType();

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xTable.Elements(XNamespace + "property"))
            {
                Properties.Add(new XmlTablePropertyMapping(Type, XNamespace, xProperty));
            }

            XElement xDiscriminator;
            if (xTable.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
            {
                Discriminator = new XmlDiscriminatorColumn(xDiscriminator);
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xTable.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlSubClassMapping(this, XNamespace, xSubClass));
            }

            XAttribute xDiscriminatorValue;
            if (xTable.TryGetAttribute("discriminator-value", out xDiscriminatorValue))
            {
                if (Discriminator == null)
                    throw new DocumentParseException("Cannot parse table discriminator value, unknown discriminator type");

                try
                {
                    DiscriminatorValue = TypeUtils.ParseAs(Discriminator.Type, xDiscriminatorValue.Value);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot parse table discriminator value '{0}' as '{1}'", xDiscriminatorValue.Value, Discriminator.Type), ex);
                }
            }

            XElement xVersionProperty;
            if (xTable.TryGetElement(XNamespace + "version", out xVersionProperty))
            {
                VersionProperty = new XmlVersionProperty(this, xVersionProperty);
            }

            XElement xPrimaryKey;
            if (!xTable.TryGetElement(XNamespace + "primary-key", out xPrimaryKey)) 
                return;

            PrimaryKeyProperties = new List<IPropertyMapping>();

            var properties = Properties.ToDictionary(p => p.Name);

            foreach (var xProperty in xPrimaryKey.Elements(XNamespace + "property"))
            {
                var name = xProperty.Attribute("name").Value;

                IPropertyMapping propertyMapping;
                if (!properties.TryGetValue(name, out propertyMapping))
                    throw new DocumentParseException("Cannot find primary key property '{0}'", name);

                PrimaryKeyProperties.Add(propertyMapping);
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IDiscriminatorColumn Discriminator { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }

        public IVersionProperty VersionProperty { get; private set; }

        public IList<IPropertyMapping> PrimaryKeyProperties { get; private set; }
    }
}