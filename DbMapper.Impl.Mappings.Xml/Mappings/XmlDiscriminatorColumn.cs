using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlDiscriminatorColumnMapping : IDiscriminatorColumnMapping
    {
        public XmlDiscriminatorColumnMapping(XElement xDiscriminator)
        {
            XAttribute xType;
            if (!xDiscriminator.TryGetAttribute("type", out xType))
                throw new DocumentParseException("Cannot find type at discriminator");

            var typeString = xType.Value;

            try
            {
                Type = TypeUtils.ParseType(typeString, true);
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize discriminator type '{0}'", typeString), ex);
            }

            XAttribute xColumn;
            if (!xDiscriminator.TryGetAttribute("column", out xColumn))
                throw new DocumentParseException("Cannot find column at discriminator");

            Column = xColumn.Value;
        }

        public Type Type { get; private set; }

        public string Column { get; private set; }
    }
}