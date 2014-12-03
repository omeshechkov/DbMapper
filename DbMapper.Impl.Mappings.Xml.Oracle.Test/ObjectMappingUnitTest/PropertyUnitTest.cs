using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test.ObjectMappingUnitTest
{
    [TestFixture]
    public class PropertyUnitTest
    {
        [Test]
        public void NoPropertyAttribute()
        {
            var xml = XElement.Parse("<property name='X' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find attribute at object property mapping", ex.Message);
        }

        [Test]
        public void NoPropertyName()
        {
            var xml = XElement.Parse("<property attribute='x' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find name at object property mapping", ex.Message);
        }

        [Test]
        public void WrongPropertyName()
        {
            var xml = XElement.Parse("<property name='XX' attribute='x' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual(string.Format("Canot find member 'XX' of type '{0}'", typeof(Shape).AssemblyQualifiedName), ex.Message);
        }
        
        [Test]
        public void PublicPropertyName()
        {
            var xml = XElement.Parse("<property name='X' attribute='x' />");

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);
            var propertyInfo = typeof(Shape).GetMember("X", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, mapping.Member);
        }

        [Test]
        public void PrivatePropertyName()
        {
            var xml = XElement.Parse("<property name='Y' attribute='y' />");

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);
            var propertyInfo = typeof(Shape).GetMember("Y", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(propertyInfo, mapping.Member);
        }

        [Test]
        public void PublicFieldName()
        {
            var xml = XElement.Parse("<property name='_y' attribute='y' />");

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);
            var propertyInfo = typeof(Shape).GetMember("_y", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, mapping.Member);
        }

        [Test]
        public void PrivateFieldName()
        {
            var xml = XElement.Parse("<property name='_x' attribute='x' />");

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);
            var propertyInfo = typeof(Shape).GetMember("_x", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(propertyInfo, mapping.Member);
        }

        [Test]
        public void NoPropertyConverterName()
        {
            var xml = XElement.Parse("<property name='X' attribute='x' />");

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);
            
            Assert.AreEqual(null, mapping.Converter);
        }
        
        [Test]
        public void CheckPropertyConverter()
        {
            var xml = XElement.Parse(string.Format("<property name='X' attribute='x' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));

            var mapping = new XmlObjectPropertyMapping(typeof(Shape), xml);

            Assert.IsInstanceOf<YesNoConverter>(mapping.Converter);
        }

        [Test]
        public void CheckPropertyPseudoConverter()
        {
            var pseudoConverterType = typeof(PseudoConverter).AssemblyQualifiedName;
            var iConverterType = typeof(IConverter).AssemblyQualifiedName;
            var xml = XElement.Parse(string.Format("<property name='X' attribute='x' converter='{0}' />", pseudoConverterType));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectPropertyMapping(typeof(Shape), xml));
            Assert.AreEqual(string.Format("Illegal converter class '{0}', class must be inherited from '{1}'", pseudoConverterType, iConverterType), ex.Message);
        }
    }
}
