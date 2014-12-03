using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.MappingBuilders;
using log4net;

namespace DbMapper.Impl.Mappings.Xml
{
    public class XmlMappingBuilder : DefaultMappingBuilder
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(XmlMappingBuilder));

        static XmlMappingBuilder()
        {
            var assembly = Assembly.GetAssembly(typeof(XmlMappingBuilder));

            var tableMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.XSD.TableMapping.xsd");
            if (tableMappingSchemaStream == null)
                throw new Exception("Cannot find table mapping schema");

            var viewMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.XSD.ViewMapping.xsd");
            if (viewMappingSchemaStream == null)
                throw new Exception("Cannot find view mapping schema");

            RegisterMappingBuilder("table-mapping", new StreamReader(tableMappingSchemaStream), xMapping => new XmlTableMapping(xMapping));
            RegisterMappingBuilder("view-mapping", new StreamReader(viewMappingSchemaStream),  xMapping => new XmlViewMapping(xMapping));

            ConverterFactory.RegisterShorthand<YesNoConverter>("YN");
            ConverterFactory.RegisterShorthand<LowerYesNoConverter>("yn");

            ConverterFactory.RegisterShorthand<TrueFalseConverter>("TF");
            ConverterFactory.RegisterShorthand<LowerTrueFalseConverter>("tf");
        }

        protected static void RegisterMappingBuilder(string documentType, TextReader schema, MappingBuilder builder)
        {
            MappingFactory.RegisterMapping(documentType, schema, builder);
        }

        public sealed override void Configure(XElement configuration)
        {
            //TODO - validate using schema
            //configuration.Validate();

            var source = configuration.Elements().First();

            if (source.Name == "assembly-resources")
            {
                var assemblyName = source.Attribute("name").Value;
                var resourcesMask = source.Attribute("mask").Value;

                LoadFromAssembly(assemblyName, resourcesMask);
            }
        }

        private void LoadFromAssembly(string name, string mask)
        {
            var assembly = Assembly.Load(name);

            var regex = new Regex(mask);

            var resourceNames = assembly.GetManifestResourceNames().Where(n => regex.IsMatch(n));

            foreach (var resourceName in resourceNames)
            {
                var resourceStream = assembly.GetManifestResourceStream(resourceName);
                if (resourceStream == null)
                    throw new DocumentLoadException("Cannot load resource '{0}', stream is null", resourceName);

                var streamReader = new StreamReader(resourceStream);
                try
                {
                    var xMapping = XDocument.Parse(streamReader.ReadToEnd());

                    var mapping = MappingFactory.CreateMapping(xMapping);

                    RegisterMapping(mapping);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot load resource '{0}'", resourceName), ex);
                }
            }
        }
    }
}