using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace DbMapper.Configuration
{
    public sealed class DbMapperSection : IConfigurationSectionHandler
    {
        public const string Name = "db-mapper";

        public object Create(object parent, object configContext, XmlNode section)
        {
            return XElement.Parse(section.OuterXml);
        }
    }
}
