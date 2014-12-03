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
    sealed class XmlTablePropertyMapping : ITablePropertyMapping
    {
        public XmlTablePropertyMapping(Type classType, XElement xTableProperty)
        {
            XAttribute xColumn;
            if (!xTableProperty.TryGetAttribute("column", out xColumn))
                throw new DocumentParseException("Cannot find column at table property mapping");

            Name = xColumn.Value;

            XAttribute xName;
            if (!xTableProperty.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at table property mapping");

            var name = xName.Value;

            Member = classType.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

            if (Member == null)
                throw new DocumentParseException("Canot find member '{0}' of type '{1}'", name, classType.AssemblyQualifiedName);

            XElement xGenerator;
            if (xTableProperty.TryGetElement(XmlTableMapping.XNamespace + "generator", out xGenerator))
            {
                var xGeneratorElement = xGenerator.SubElement();
                if (xGeneratorElement == null)
                    throw new DocumentParseException("No generator type at table property mapping");

                Generator = GeneratorFactory.GetGenerator(xGeneratorElement);
            }

            XAttribute xInsert;
            Insert = !xTableProperty.TryGetAttribute("insert", out xInsert) || xInsert.GetValueAsBoolean();

            XAttribute xUpdate;
            Update = !xTableProperty.TryGetAttribute("update", out xUpdate) || xUpdate.GetValueAsBoolean();

            XAttribute xConverter;
            if (xTableProperty.TryGetAttribute("converter", out xConverter))
            {
                Converter = ConverterFactory.Create(xConverter.Value);
            }
        }

        public string Name { get; private set; }

        public MemberInfo Member { get; private set; }

        public IGenerator Generator { get; private set; }

        public bool Insert { get; private set; }

        public bool Update { get; private set; }

        public IConverter Converter { get; set; }
    }
}