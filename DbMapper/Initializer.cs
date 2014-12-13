using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using DbMapper.Configuration;
using DbMapper.Utils;

namespace DbMapper
{
    public static class Initializer
    {
        public static void Initialize()
        {
            try
            {
                var xConfiguration = (XElement)ConfigurationManager.GetSection(DbMapperSection.Name);

                XElement xModules;
                if (!xConfiguration.TryGetElement("modules", out xModules))
                    throw new Exceptions.ConfigurationException("Cannot find modules, check your configuration");

                IList<XElement> xModuleList;
                if (!xModules.TryGetElements("module", out xModuleList))
                    throw new Exceptions.ConfigurationException("Cannot find modules, check your configuration");

                foreach (var xModule in xModuleList)
                {
                    XAttribute xModuleClass;
                    if (!xModule.TryGetAttribute("class", out xModuleClass))
                        throw new Exceptions.ConfigurationException("Cannot find module class, check your configuration");

                    Type moduleType;
                    try
                    {
                        moduleType = xModuleClass.GetValueAsType();
                    }
                    catch (Exception ex)
                    {
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load module '{0}'", xModuleClass.Value), ex);
                    }

                    if (!typeof(IDbModule).IsAssignableFrom(moduleType))
                        throw new Exceptions.ConfigurationException(string.Format("Cannot load module '{0}', module must be inherited from '{1}'", xModuleClass.Value,
                            typeof(IDbModule).AssemblyQualifiedName));

                    var module = (IDbModule)Activator.CreateInstance(moduleType);

                    module.Configure(xModule);
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize db-maper", ex);
            }
        }
    }
}