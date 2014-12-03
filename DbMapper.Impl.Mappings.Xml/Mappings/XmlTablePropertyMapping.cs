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
        public XmlTablePropertyMapping(Type classType, XNamespace xNamespace, XElement xTableProperty)
        {
            Name = xTableProperty.Attribute("column").Value;
            
            var name = xTableProperty.Attribute("name").Value;

            Member = classType.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

            if (Member == null)
                throw new DocumentParseException("Canot find member '{0}'", name);

            XElement xGenerator;
            if (xTableProperty.TryGetElement(xNamespace + "generator", out xGenerator))
            {
                var xGeneratorElement = xGenerator.SubElement();
                Generator = GeneratorFactory.GetGenerator(xNamespace, xGeneratorElement);
            }

            XAttribute xInsert;
            Insert = !xTableProperty.TryGetAttribute("insert", out xInsert) || xInsert.GetAsBoolean();

            XAttribute xUpdate;
            Update = !xTableProperty.TryGetAttribute("update", out xUpdate) || xUpdate.GetAsBoolean();

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