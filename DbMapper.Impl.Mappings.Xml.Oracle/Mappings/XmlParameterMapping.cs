using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Oracle.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlParameterMapping : IParameterMapping
    {
        public XmlParameterMapping(MethodInfo methodInfo, XElement xParameter)
        {
            XAttribute xDbName;
            if (!xParameter.TryGetAttribute("db-name", out xDbName))
                throw new DocumentParseException("Cannot find db-name at function mapping");

            DbParameterName = xDbName.Value;

            XAttribute xName;
            if (!xParameter.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at function mapping");

            var parameterName = xName.Value;

            Parameter = methodInfo.GetParameters().FirstOrDefault(p => p.Name == parameterName);

            if (Parameter == null)
                throw new DocumentParseException("Cannot find parameter '{0}' at function parameter mapping", parameterName);

            XAttribute xConverter;
            if (xParameter.TryGetAttribute("converter", out xConverter))
            {
                Converter = ConverterFactory.Create(xConverter.Value);
            }

            XAttribute xDbType;
            if (xParameter.TryGetAttribute("db-type", out xDbType))
            {
                DbType = xDbType.GetValueAsEnum<DbType>();
            }
            else
            {
                DbType = (DbType)(-1);
            }

            XAttribute xLength;
            if (xParameter.TryGetAttribute("length", out xLength))
            {
                Length = xLength.GetValueAsInt();
            }
        }

        public IConverter Converter { get; set; }

        public ParameterInfo Parameter { get; private set; }

        public string DbParameterName { get; private set; }

        public DbType DbType { get; set; }

        public int Length { get; private set; }
    }
}