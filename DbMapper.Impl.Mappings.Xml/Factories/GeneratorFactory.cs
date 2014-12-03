using System.Xml.Linq;
using DbMapper.Generators;
using DbMapper.Impl.Mappings.Xml.Exceptions;

namespace DbMapper.Impl.Mappings.Xml.Factories
{
    internal static class GeneratorFactory
    {
        public static IGenerator GetGenerator(XNamespace xNamespace, XElement xGeneratorElement)
        {
            var generatorName = xGeneratorElement.Name;

            if (generatorName == xNamespace + "sequence")
            {
                var sequenceName = xGeneratorElement.Attribute("name").Value;
                return new SequenceGenerator(sequenceName);
            }

            if (generatorName == xNamespace + "db-assigned")
            {
                return DbAssignedGenerator.Instance;
            }

            throw new DocumentParseException("Unknown generator type '{0}'", generatorName);
        }
    }
}