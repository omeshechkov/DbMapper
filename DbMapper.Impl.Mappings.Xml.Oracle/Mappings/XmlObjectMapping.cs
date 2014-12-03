using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;
using DbMapper.Oracle.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlObjectMapping : IObjectMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-object-mapping";

        public XmlObjectMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build object mapping", new ArgumentNullException("xMapping"));                


            XElement xObject;

            if (!xMapping.TryGetElement(XNamespace + "object", out xObject))
                throw new DocumentParseException("Cannot find object at object mapping");

            XAttribute xSchema;
            if (xObject.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xObject.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at object mapping");

            Name = xName.Value;

            XAttribute xClass;
            if (!xObject.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at object mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at object mapping", xClass.Value), ex);
            }

            Properties = new List<IPropertyMapping>();
            foreach (var xProperty in xObject.Elements(XNamespace + "property"))
            {
                Properties.Add(new XmlObjectPropertyMapping(Type, xProperty));
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IPropertyMapping> Properties { get; private set; }
    }

    sealed class XmlObjectPropertyMapping : IObjectPropertyMapping
    {
        public XmlObjectPropertyMapping(Type classType, XElement xObjectProperty)
        {
            XAttribute xAttribute;
            if (!xObjectProperty.TryGetAttribute("attribute", out xAttribute))
                throw new DocumentParseException("Cannot find attribute at object property mapping");

            Name = xAttribute.Value;

            XAttribute xName;
            if (!xObjectProperty.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at object property mapping");

            var name = xName.Value;

            Member = classType.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

            if (Member == null)
                throw new DocumentParseException("Canot find member '{0}' of type '{1}'", name, classType.AssemblyQualifiedName);

            XAttribute xConverter;
            if (xObjectProperty.TryGetAttribute("converter", out xConverter))
            {
                Converter = ConverterFactory.Create(xConverter.Value);
            }
        }

        public IConverter Converter { get; set; }

        public string Name { get; private set; }

        public MemberInfo Member { get; private set; }
    }
}