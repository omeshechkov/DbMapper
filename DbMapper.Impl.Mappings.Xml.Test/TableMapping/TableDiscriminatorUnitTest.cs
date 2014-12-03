using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableDiscriminatorUnitTest
    {
        [Test]
        public void NoDiscriminatorColumn()
        {
            var xml = XElement.Parse("<discriminator type='long' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlDiscriminatorColumn(xml));
            Assert.AreEqual("Cannot find column at discriminator", ex.Message);
        }

        [Test]
        public void NoDiscriminatorColumnType()
        {
            var xml = XElement.Parse("<discriminator column='type' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlDiscriminatorColumn(xml));
            Assert.AreEqual("Cannot find type at discriminator", ex.Message);
        }

        [Test]
        public void WrongDiscriminatorColumnType()
        {
            var xml = XElement.Parse("<discriminator column='type' type='UnknownType' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlDiscriminatorColumn(xml));
            Assert.AreEqual("Cannot recognize discriminator type 'UnknownType'", ex.Message);
        }

        [Test]
        public void CheckDiscriminatorColumnName()
        {
            var xml = XElement.Parse("<discriminator column='type' type='long' />");

            var discriminator = new XmlDiscriminatorColumn(xml);
            Assert.AreEqual("type", discriminator.Column);
        }

        [Test]
        public void CheckDiscriminatorColumnType()
        {
            var xml = XElement.Parse("<discriminator column='type' type='string' />");

            var discriminator = new XmlDiscriminatorColumn(xml);
            Assert.AreEqual(typeof(string), discriminator.Type);
        }
    }
}