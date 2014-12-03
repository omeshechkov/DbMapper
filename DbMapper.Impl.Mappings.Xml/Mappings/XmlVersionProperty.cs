using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlVersionProperty : IVersionProperty
    {
        public XmlVersionProperty(Type classType, XElement xVersion)
        {
            XAttribute xColumn;
            if (!xVersion.TryGetAttribute("column", out xColumn))
                throw new DocumentParseException("Cannot find column at version property mapping");

            Name = xColumn.Value;

            XAttribute xName;
            if (!xVersion.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at version property mapping");

            var name = xName.Value;

            Member = classType.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

            if (Member == null)
                throw new DocumentParseException("Canot find member '{0}' of type '{1}'", name, classType.AssemblyQualifiedName);

            XAttribute xConverter;
            if (xVersion.TryGetAttribute("converter", out xConverter))
            {
                Converter = ConverterFactory.Create(xConverter.Value);
            }
        }

        public string Name { get; private set; }

        public MemberInfo Member { get; private set; }

        public IConverter Converter { get; set; }
    }
}