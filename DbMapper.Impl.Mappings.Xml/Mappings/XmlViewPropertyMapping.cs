using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlViewPropertyMapping : IViewPropertyMapping
    {
        public XmlViewPropertyMapping(Type classType, XElement xViewProperty)
        {
            XAttribute xColumn;
            if (!xViewProperty.TryGetAttribute("column", out xColumn))
                throw new DocumentParseException("Cannot find column at view property mapping");

            Name = xColumn.Value;

            XAttribute xName;
            if (!xViewProperty.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at view property mapping");

            var name = xName.Value;

            Member = classType.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

            if (Member == null)
                throw new DocumentParseException("Canot find member '{0}' of type '{1}'", name, classType.AssemblyQualifiedName);

            XAttribute xConverter;
            if (xViewProperty.TryGetAttribute("converter", out xConverter))
            {
                Converter = ConverterFactory.Create(xConverter.Value);
            }
        }


        public string Name { get; private set; }

        public MemberInfo Member { get; private set; }

        public IConverter Converter { get; set; }
    }
}