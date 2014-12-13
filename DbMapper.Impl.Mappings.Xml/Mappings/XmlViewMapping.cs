using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlViewMapping : IViewMapping
    {
        public static readonly XNamespace XNamespace = "urn:dbm-view-mapping";

        public XmlViewMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build view mapping", new ArgumentNullException("xMapping"));

            XElement xView;
            if (!xMapping.TryGetElement(XNamespace + "view", out xView))
                throw new DocumentParseException("Cannot find view at view mapping");

            XAttribute xSchema;
            if (xView.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xView.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at view mapping");

            Name = xName.Value;

            XAttribute xClass;
            if (!xView.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at view mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at view mapping", xClass.Value), ex);
            }

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xView.Elements(XNamespace + "property"))
            {
                Properties.Add(new XmlViewPropertyMapping(Type, xProperty));
            }

            XElement xDiscriminator;
            if (xView.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
            {
                Discriminator = new XmlDiscriminatorMapping(xDiscriminator);
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xView.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlViewSubClassMapping(this, Discriminator, xSubClass));
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
                    throw new DocumentParseException(string.Format("Cannot parse view discriminator value '{0}' as '{1}'", xDiscriminatorValue.Value, Discriminator.Type.AssemblyQualifiedName), ex);
                }
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IDiscriminatorMapping Discriminator { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }
    }
}