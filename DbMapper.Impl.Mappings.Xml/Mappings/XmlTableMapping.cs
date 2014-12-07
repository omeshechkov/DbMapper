using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlTableMapping : ITableMapping
    {
        internal static readonly XNamespace XNamespace = "urn:dbm-table-mapping";

        public XmlTableMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build table mapping", new ArgumentNullException("xMapping"));

            XElement xTable;
            if (!xMapping.TryGetElement(XNamespace + "table", out xTable))
                throw new DocumentParseException("Cannot find table at table mapping");

            XAttribute xSchema;
            if (xTable.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xTable.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at table mapping");

            Name = xName.Value;

            XAttribute xClass;
            if (!xTable.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at table mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at table mapping", xClass.Value), ex);
            }

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xTable.Elements(XNamespace + "property"))
            {
                Properties.Add(new XmlTablePropertyMapping(Type, xProperty));
            }

            XElement xDiscriminator;
            if (xTable.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
            {
                Discriminator = new XmlDiscriminatorColumnMapping(xDiscriminator);
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xTable.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlTableSubClassMapping(Discriminator, xSubClass));
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
                    throw new DocumentParseException(string.Format("Cannot parse table discriminator value '{0}' as '{1}'", xDiscriminatorValue.Value, Discriminator.Type.AssemblyQualifiedName), ex);
                }
            }

            XElement xVersionProperty;
            if (xTable.TryGetElement(XNamespace + "version", out xVersionProperty))
            {
                Version = new XmlVersionPropertyMapping(Type, xVersionProperty);
            }

            XElement xPrimaryKey;
            if (!xTable.TryGetElement(XNamespace + "primary-key", out xPrimaryKey)) 
                return;

            PrimaryKeyProperties = new List<IPropertyMapping>();

            var properties = Properties.ToDictionary(p => p.Name);

            foreach (var xProperty in xPrimaryKey.Elements(XNamespace + "property"))
            {
                XAttribute xPrimaryKeyPropertyName;
                if (!xProperty.TryGetAttribute("name", out xPrimaryKeyPropertyName))
                    throw new DocumentParseException("Cannot find name at table primary key mapping");

                var name = xPrimaryKeyPropertyName.Value;

                IPropertyMapping propertyMapping;
                if (!properties.TryGetValue(name, out propertyMapping))
                    throw new DocumentParseException("Cannot find primary key property '{0}' at table primary key mapping", name);

                PrimaryKeyProperties.Add(propertyMapping);
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IDiscriminatorColumnMapping Discriminator { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }

        public IVersionPropertyMapping Version { get; private set; }

        public IList<IPropertyMapping> PrimaryKeyProperties { get; private set; }
    }
}