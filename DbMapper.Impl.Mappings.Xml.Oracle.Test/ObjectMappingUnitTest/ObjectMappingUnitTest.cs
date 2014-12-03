using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test.ObjectMappingUnitTest
{
    [TestFixture]
    public class ObjectMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectMapping(null));
            Assert.AreEqual("Cannot build object mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoObjectMapping()
        {
            var xml = XElement.Parse("<object-mapping xmlns='urn:dbm-oracle-object-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectMapping(xml));
            Assert.AreEqual("Cannot find object at object mapping", ex.Message);
        }

        [Test]
        public void NoObjectSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object name='shape' class='{0}' />
</object-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectMapping(xml);
            Assert.IsNull(mapping.Schema);
        }
        
        [Test]
        public void NoObjectName()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' class='{0}' />
</object-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectMapping(xml));
            Assert.AreEqual("Cannot find name at object mapping", ex.Message);
        }

        [Test]
        public void NoObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' name='shape' />
</object-mapping>
"));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectMapping(xml));
            Assert.AreEqual("Cannot find class at object mapping", ex.Message);
        }

        [Test]
        public void WrongObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' name='shape' class='WrongClass' />
</object-mapping>
"));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectMapping(xml));
            Assert.AreEqual("Cannot recognize 'WrongClass' class at object mapping", ex.Message);
        }
        
        [Test]
        public void CheckObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' name='shape' class='{0}' />
</object-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.Type);
        }

        [Test]
        public void CheckObjectSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' name='shape' class='{0}' />
</object-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectMapping(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }
        
        [Test]
        public void CheckObjectProperties()
        {
            var xml = XElement.Parse(string.Format(@"
<object-mapping xmlns='urn:dbm-oracle-object-mapping'>
  <object schema='test_dbm' name='shape' class='{0}'>
    <property name='X' attribute='x' />
    
    <property name='Y' attribute='y' />
  </object>
</object-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectMapping(xml);
            Assert.AreEqual(2, mapping.Properties.Count);
        }
    }
}