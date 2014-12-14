using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlExtendTableMapping : IExtendTableMapping
    {
        internal static readonly XNamespace XNamespace = "urn:dbm-extend-table-mapping";

        public XmlExtendTableMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build extend-table mapping", new ArgumentNullException("xMapping"));

            XElement xExtendTable;
            if (!xMapping.TryGetElement(XNamespace + "extend-table", out xExtendTable))
                throw new DocumentParseException("Cannot find extend-table at extend-table mapping");

            XAttribute xClass;
            if (!xExtendTable.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at extend-table mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at extend-table mapping", xClass.Value), ex);
            }

            XElement xDiscriminator;
            if (xExtendTable.TryGetElement(XNamespace + "discriminator", out xDiscriminator))
                Discriminator = new XmlDiscriminatorMapping(xDiscriminator);

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubClass in xExtendTable.Elements(XNamespace + "subclass"))
            {
                SubClasses.Add(new XmlTableSubClassMapping(this, Discriminator, xSubClass));
            }
        }

        public Type Type { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public IDiscriminatorMapping Discriminator { get; private set; }
    }
}