using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlTableSubClassMapping : ISubClassMapping
    {
        public XmlTableSubClassMapping(IDiscriminatorColumnMapping discriminatorColumn, XElement xSubClass)
        {
            var xNamespace = xSubClass.Name.Namespace;

            XAttribute xName;
            if (!xSubClass.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at table subclass mapping");

            try
            {
                Type = xName.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize subclass '{0}' at table subclass mapping", xName.Value), ex);
            }

            XElement xSubClassJoin;
            if (xSubClass.TryGetElement(xNamespace + "join", out xSubClassJoin))
            {
                Join = new XmlSubClassJoin(xNamespace, xSubClassJoin);
            }

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xSubClass.Elements(xNamespace + "property"))
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
            foreach (var xSubSubClass in xSubClass.Elements(xNamespace + "subclass"))
            {
                SubClasses.Add(new XmlTableSubClassMapping(discriminatorColumn, xSubSubClass));
            }
        }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public object DiscriminatorValue { get; private set; }

        public ISubClassJoin Join { get; private set; }
    }
}