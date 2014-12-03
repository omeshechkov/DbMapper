using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlViewMapping : IViewMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-view-mapping";

        public XmlViewMapping(XContainer xMapping)
        {
            var xView = xMapping.Element(XNamespace + "view");

            XAttribute xSchema;
            if (xView.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            Name = xView.Attribute("name").Value;

            Type = xView.Attribute("class").GetValueAsType();

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xView.Elements(XNamespace + "property"))
            {
                Properties.Add(new XmlViewPropertyMapping(Type, xProperty));
            }

            XElement xDiscriminator;
            if (xView.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
            {
                Discriminator = new XmlDiscriminatorColumn(xDiscriminator);
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xView.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlSubClassMapping(this, XNamespace, xSubClass));
            }

            XAttribute xDiscriminatorValue;
            if (xView.TryGetAttribute("discriminator-value", out xDiscriminatorValue))
            {
                if (Discriminator == null)
                    throw new DocumentParseException("Cannot parse view discriminator value, unknown discriminator type");

                try
                {
                    DiscriminatorValue = TypeUtils.ParseAs(Discriminator.Type, xDiscriminatorValue.Value);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot parse view discriminator value '{0}' as '{1}'", xDiscriminatorValue.Value, Discriminator.Type), ex);
                }
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IDiscriminatorColumn Discriminator { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }
    }
}