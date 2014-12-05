using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping
{
    [TestFixture]
    class ViewPropertyMappingUnitTest
    {
        [Test]
        public void NoName()
        {
            var xml = XElement.Parse("<property column='id' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find name at view property mapping", ex.Message);
        }

        [Test]
        public void NoColumn()
        {
            var xml = XElement.Parse("<property name='Id' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find column at view property mapping", ex.Message);
        }

        [Test]
        public void NoConverter()
        {
            var xml = XElement.Parse("<property name='Id' column='id' />");
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            Assert.IsNull(mapping.Converter);
        }

        [Test]
        public void CheckConverter()
        {
            var xml = XElement.Parse(string.Format("<property name='Version' column='version' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            Assert.IsInstanceOf<YesNoConverter>(mapping.Converter);
        }

        [Test]
        public void CheckPublicFieldReference()
        {
            var xml = XElement.Parse("<property name='_version' column='version' />");
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_version", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPrivateFieldReference()
        {
            var xml = XElement.Parse("<property name='_id' column='id' />");
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_id", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPublicPropertyReference()
        {
            var xml = XElement.Parse("<property name='Id' column='version' />");
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("Id", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPrivatePropertyReference()
        {
            var xml = XElement.Parse("<property name='Version' column='version' />");
            var mapping = new XmlViewPropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("Version", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }
   }
}