using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlExtendViewMapping : IExtendTableMapping
    {
        internal static readonly XNamespace XNamespace = "urn:dbm-extend-view-mapping";

        public XmlExtendViewMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build extend-view mapping", new ArgumentNullException("xMapping"));

            XElement xExtendView;
            if (!xMapping.TryGetElement(XNamespace + "extend-view", out xExtendView))
                throw new DocumentParseException("Cannot find extend-view at extend-view mapping");

            XAttribute xClass;
            if (!xExtendView.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at extend-view mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at extend-view mapping", xClass.Value), ex);
            }

            XElement xDiscriminator;
            if (!xExtendView.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
                throw new DocumentParseException("Cannot find discriminator at extend-view mapping");

            Discriminator = new XmlDiscriminatorColumnMapping(xDiscriminator);

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xExtendView.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlViewSubClassMapping(Discriminator, xSubClass));
            }
        }

        public Type Type { get; private set; }

        public IDiscriminatorColumnMapping Discriminator { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }
    }
}