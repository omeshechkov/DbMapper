using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Oracle.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlObjectTableMapping : IObjectTableMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-object-table-mapping";

        public XmlObjectTableMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build object table mapping", new ArgumentNullException("xMapping"));


            XElement xObjectTable;

            if (!xMapping.TryGetElement(XNamespace + "object-table", out xObjectTable))
                throw new DocumentParseException("Cannot find object-table at object table mapping");

            XAttribute xSchema;
            if (xObjectTable.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xObjectTable.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at object table mapping");

            Name = xName.Value;

            XAttribute xClass;
            if (!xObjectTable.TryGetAttribute("class", out xClass))
                throw new DocumentParseException("Cannot find class at object table mapping");

            try
            {
                Type = xClass.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at object table mapping", xClass.Value), ex);
            }

            XAttribute xObjectClass;
            if (!xObjectTable.TryGetAttribute("object-class", out xObjectClass))
                throw new DocumentParseException("Cannot find object-class at object table mapping");

            var objectTypeString = xObjectClass.Value;

            try
            {
                ObjectType = TypeUtils.ParseType(objectTypeString, false);
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' object-class at object table mapping", objectTypeString), ex);
            }            
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public Type ObjectType { get; set; }
    }
}