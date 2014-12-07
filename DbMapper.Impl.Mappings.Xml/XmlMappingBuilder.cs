using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;
using DbMapper.Utils;
using log4net;

namespace DbMapper.Impl.Mappings.Xml
{
    public class XmlMappingBuilder : IStaticMappingBuilder
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

        public void Configure(XElement configuration)
        {
            var xSource = configuration.SubElement();

            if (xSource.Name == "assembly-resources")
            {
                XAttribute xName;
                if (!xSource.TryGetAttribute("name", out xName))
                    throw new ConfigurationException("Cannot find name at assembly-resources");
                
                var assemblyName = xName.Value;

                if (string.IsNullOrEmpty(assemblyName))
                    throw new ConfigurationException("Name is empty at assembly-resources");

                XAttribute xMask;
                if (!xSource.TryGetAttribute("mask", out xMask))
                    throw new ConfigurationException("Cannot find mask at assembly-resources");

                var resourcesMask = xMask.Value;

                if (string.IsNullOrEmpty(resourcesMask))
                    throw new ConfigurationException("Mask is empty at assembly-resources");

                LoadFromAssembly(assemblyName, resourcesMask);
            }
        }

        public ICollection<IMappingClassReference> Mappings { get; private set; }

        private void LoadFromAssembly(string name, string mask)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            
            if (mask == null)
                throw new ArgumentNullException("mask");
            
            Mappings = new List<IMappingClassReference>();

            Assembly assembly;
            try
            {
                assembly = Assembly.Load(name);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(string.Format("Cannot load xml mappings from assembly '{0}'", name), ex);
            }

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

                    Mappings.Add(mapping);
                }
                catch (Exception ex)
                {
                    throw new DocumentParseException(string.Format("Cannot load resource '{0}'", resourceName), ex);
                }
            }
        }
    }
}