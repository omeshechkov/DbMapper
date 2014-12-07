using System.Xml.Linq;
using DbMapper.Generators;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Factories
{
    internal static class GeneratorFactory
    {
        public static IGenerator GetGenerator(XElement xGeneratorElement)
        {
            var generatorName = xGeneratorElement.Name;

            if (generatorName == XmlTableMapping.XNamespace + "sequence")
            {
                XAttribute xName;
                if (!xGeneratorElement.TryGetAttribute("name", out xName))
                    throw new DocumentParseException("Cannot find name at sequence generator");

                return new SequenceGenerator(xName.Value);
            }

            if (generatorName == XmlTableMapping.XNamespace + "db-assigned")
            {
                return DbAssignedGenerator.Instance;
            }

            throw new DocumentParseException("Unknown generator type '{0}'", generatorName);
        }
    }
}