using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableVersionPropertyMappingUnitTest
    {
        [Test]
        public void NoVersionPropertyName()
        {
            var xml = XElement.Parse("<version column='version' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlVersionProperty(typeof(Shape), xml));
            Assert.AreEqual("Cannot find name at version property mapping", ex.Message);
        }

        [Test]
        public void NoVersionColumn()
        {
            var xml = XElement.Parse("<version name='Version' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlVersionProperty(typeof(Shape), xml));
            Assert.AreEqual("Cannot find column at version property mapping", ex.Message);
        }

        [Test]
        public void CheckVersionColumnName()
        {
            var xml = XElement.Parse("<version name='Version' column='version' />");

            var property = new XmlVersionProperty(typeof(Shape), xml);
            Assert.AreEqual("version", property.Name);
        }

        [Test]
        public void NoVersionConverter()
        {
            var xml = XElement.Parse("<version name='Version' column='version' />");

            var property = new XmlVersionProperty(typeof(Shape), xml);
            Assert.IsNull(property.Converter);
        }

        [Test]
        public void CheckVersionConverter()
        {
            var xml = XElement.Parse(string.Format("<version name='Version' column='version' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));

            var property = new XmlVersionProperty(typeof(Shape), xml);
            Assert.IsInstanceOf<YesNoConverter>(property.Converter);
        }

        [Test]
        public void CheckVersionPrivateFieldReference()
        {
            var xml = XElement.Parse("<version name='_id' column='id' />");
            var mapping = new XmlVersionProperty(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_id", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

         [Test]
         public void CheckVersionPrivatePropertyReference()
         {
             var xml = XElement.Parse("<version name='Version' column='version' />");
             var mapping = new XmlVersionProperty(typeof(Shape), xml);
             var fieldInfo = typeof(Shape).GetMember("Version", BindingFlags.Instance | BindingFlags.NonPublic).First();
             Assert.AreEqual(fieldInfo, mapping.Member);
         }

        [Test]
        public void CheckVersionPublicFieldReference()
        {
            var xml = XElement.Parse("<version name='_version' column='version' />");
            var mapping = new XmlVersionProperty(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_version", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckVersionPublicPropertyReference()
        {
            var xml = XElement.Parse("<version name='Id' column='id' />");
            var mapping = new XmlVersionProperty(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("Id", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }
    }
}