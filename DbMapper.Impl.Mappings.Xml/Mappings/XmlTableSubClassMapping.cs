using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlTableSubClassMapping : ITableSubClassMapping
    {
        public XmlTableSubClassMapping(IMappingClassReference parent, IDiscriminatorMapping discriminator, XElement xSubClass)
        {
            Parent = parent;

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
                if (discriminator == null)
                    throw new DocumentParseException("Cannot parse discriminator value at table subclass mapping, unknown discriminator type");

                try
                {
                    DiscriminatorValue = TypeUtils.ParseAs(discriminator.Type, xDiscriminatorValue.Value);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(
                        string.Format("Cannot parse subclass discriminator value '{0}' as '{1}' at table subclass mapping", xDiscriminatorValue.Value,
                            discriminator.Type.AssemblyQualifiedName), ex);
                }
            }
            else
            {
                if (Join == null)
                    throw new DocumentParseException("Cannot parse table subclass, you have to specify discriminator value or join");
            }

            SubClasses = new List<ISubClassMapping>();
            foreach (var xSubSubClass in xSubClass.Elements(xNamespace + "subclass"))
            {
                SubClasses.Add(new XmlTableSubClassMapping(this, discriminator, xSubSubClass));
            }
        }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }

        public IList<ISubClassMapping> SubClasses { get; private set; }

        public IMappingClassReference Parent { get; private set; }

        public object DiscriminatorValue { get; private set; }

        public ISubClassJoin Join { get; private set; }
    }
}