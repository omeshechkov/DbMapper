using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test.ObjectTableMappingUnitTest
{
    [TestFixture]
    public class ObjectTableMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(null));
            Assert.AreEqual("Cannot build object table mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoMapping()
        {
            var xml = XElement.Parse("<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot find object-table at object table mapping", ex.Message);
        }

        [Test]
        public void NoSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table name='shape_table' class='{0}' object-class='{1}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName, typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectTableMapping(xml);
            Assert.IsNull(mapping.Schema);
        }
        
        [Test]
        public void NoName()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' class='{0}' object-class='{1}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName, typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot find name at object table mapping", ex.Message);
        }

        [Test]
        public void NoClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' object-class='{0}' />
</object-table-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot find class at object table mapping", ex.Message);
        }

        [Test]
        public void WrongClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='WrongClass' object-class='{0}' />
</object-table-mapping>
", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot recognize 'WrongClass' class at object table mapping", ex.Message);
        }
        
        [Test]
        public void CheckClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='{0}' object-class='{1}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName, typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectTableMapping(xml);
            Assert.AreEqual(typeof(Shapes), mapping.Type);
        }

        [Test]
        public void NoObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='{0}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot find object-class at object table mapping", ex.Message);
        }

        [Test]
        public void WrongObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='{0}' object-class='WrongClass' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlObjectTableMapping(xml));
            Assert.AreEqual("Cannot recognize 'WrongClass' object-class at object table mapping", ex.Message);
        }
        
        [Test]
        public void CheckObjectClass()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='{0}' object-class='{1}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName, typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectTableMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.ObjectType);
        }

        [Test]
        public void CheckSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<object-table-mapping xmlns='urn:dbm-oracle-object-table-mapping'>
  <object-table schema='test_dbm' name='shape_table' class='{0}' object-class='{1}' />
</object-table-mapping>
", typeof(Shapes).AssemblyQualifiedName, typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlObjectTableMapping(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }
    }
}