using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Oracle.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlObjectTableMapping : IObjectTableMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-object-table-mapping";

        public XmlObjectTableMapping(XElement xMapping)
        {
            var xObjectTable = xMapping.Element(XNamespace + "object-table");

            XAttribute xSchema;
            if (xObjectTable.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            Name = xObjectTable.Attribute("name").Value;

            Type = xObjectTable.Attribute("class").GetAsType();

            var objectTypeString = xObjectTable.Attribute("object-type").Value;

            ObjectType = TypeUtils.ParseType(objectTypeString, false);
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public Type ObjectType { get; set; }
    }
}