using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using DbMapper.Configuration;
using DbMapper.Factories;
using DbMapper.Utils;

namespace DbMapper
{
    public static class Initializer
    {
        private static MappingProvider _mappingProvider;

        public static IMappingProvider MappingProvider
        {
            get { return _mappingProvider; }
        }

        public static void Initialize()
        {
            try
            {
                var mappingValidatorFactory = new MappingValidatorFactory();

                _mappingProvider = new MappingProvider(mappingValidatorFactory);

                var xConfiguration = (XElement)ConfigurationManager.GetSection(DbMapperSection.Name);

                XElement xModules;
                if (!xConfiguration.TryGetElement("modules", out xModules))
                    throw new Exceptions.ConfigurationException("Cannot find modules");

                IList<XElement> xModuleList;
                if (!xModules.TryGetElements("module", out xModuleList))
                    throw new Exceptions.ConfigurationException("Cannot find modules");

                foreach (var xModule in xModuleList)
                {
                    XAttribute xClass;
                    if (!xModule.TryGetAttribute("class", out xClass))
                        throw new Exceptions.ConfigurationException("Cannot find module class");

                    Type moduleType;
                    try
                    {
                        moduleType = xClass.GetValueAsType();
                    }
                    catch (Exception ex)
                    {
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load module '{0}'", xClass.Value), ex);
                    }

                    if (!typeof(IDbModule).IsAssignableFrom(moduleType))
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load module '{0}', module must be inherited from '{1}'", xClass.Value,
                            typeof(IDbModule).AssemblyQualifiedName));

                    var module = (IDbModule)Activator.CreateInstance(moduleType);
                    //TODO
                }

                XElement xMappingBuilders;
                if (!xConfiguration.TryGetElement("mapping-builders", out xMappingBuilders))
                    throw new Exceptions.ConfigurationException("Cannot find mapping-builders");

                IList<XElement> xMappingBuildersList;
                if (!xMappingBuilders.TryGetElements("mapping-builder", out xMappingBuildersList))
                    throw new Exceptions.ConfigurationException("Cannot find mapping-builders");

                foreach (var xMappingBuilder in xMappingBuildersList)
                {
                    XAttribute xClass;
                    if (!xMappingBuilder.TryGetAttribute("class", out xClass))
                        throw new Exceptions.ConfigurationException("Cannot find mapping-builder class");

                    Type mappingBuilderType;
                    try
                    {
                        mappingBuilderType = xClass.GetValueAsType();
                    }
                    catch (Exception ex)
                    {
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load mapping-builder '{0}'", xClass.Value), ex);
                    }

                    if (!typeof(IMappingBuilder).IsAssignableFrom(mappingBuilderType))
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load mapping-builder '{0}', mapping-builder must be inherited from '{1}'", xClass.Value,
                            typeof(IMappingBuilder).AssemblyQualifiedName));

                    var mappingBuilder = (IMappingBuilder)Activator.CreateInstance(mappingBuilderType);

                    try
                    {
                        mappingBuilder.Configure(xMappingBuilder);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Cannot initialize db-maper", ex);
                    }

                    _mappingProvider.RegisterMappingBuilder(mappingBuilder);
                }

                _mappingProvider.Initialize();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize db-maper", ex);
            }
        }
    }
}