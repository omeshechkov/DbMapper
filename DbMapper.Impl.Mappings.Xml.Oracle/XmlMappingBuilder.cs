using System;
using System.IO;
using System.Reflection;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Oracle
{
    public class XmlMappingBuilder : Xml.XmlMappingBuilder
    {
        static XmlMappingBuilder()
        {
            var assembly = Assembly.GetAssembly(typeof(XmlMappingBuilder));

            var objectMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.Oracle.XSD.ObjectMapping.xsd");
            if (objectMappingSchemaStream == null)
                throw new Exception("Cannot find oracle object mapping schema");

            var objectTableMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.Oracle.XSD.ObjectTableMapping.xsd");
            if (objectTableMappingSchemaStream == null)
                throw new Exception("Cannot find oracle object-table mapping schema");

            var functionMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.Oracle.XSD.FunctionMapping.xsd");
            if (functionMappingSchemaStream == null)
                throw new Exception("Cannot find oracle function mapping schema");

            var procedureMappingSchemaStream = assembly.GetManifestResourceStream("DbMapper.Impl.Mappings.Xml.Oracle.XSD.ProcedureMapping.xsd");
            if (procedureMappingSchemaStream == null)
                throw new Exception("Cannot find oracle procedure mapping schema");

            RegisterMappingBuilder("object-mapping", new StreamReader(objectMappingSchemaStream), xMapping => new XmlObjectMapping(xMapping));
            RegisterMappingBuilder("object-table-mapping", new StreamReader(objectTableMappingSchemaStream), xMapping => new XmlObjectTableMapping(xMapping));

            RegisterMappingBuilder("function-mapping", new StreamReader(functionMappingSchemaStream), xMapping => new XmlFunctionMapping(xMapping));
            RegisterMappingBuilder("procedure-mapping", new StreamReader(procedureMappingSchemaStream), xMapping => new XmlProcedureMapping(xMapping));
        }
    }
}