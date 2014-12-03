using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Mappings
{
    sealed class XmlDiscriminatorColumn : IDiscriminatorColumn
    {
        public XmlDiscriminatorColumn(XElement xDiscriminator)
        {
            var typeString = xDiscriminator.Attribute("type").Value;

            if (string.IsNullOrEmpty(typeString))
                throw new DocumentParseException("Discriminator type is empty");

            try
            {
                Type = TypeUtils.ParseType(typeString, true);
            }
            catch (Exception ex)
            {
                throw new DocumentParseException("Wrong discriminator column type", ex);
            }

            Column = xDiscriminator.Attribute("column").Value;
        }

        public Type Type { get; private set; }

        public string Column { get; private set; }
    }
}