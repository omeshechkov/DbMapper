using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Generators;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.Converters;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TablePropertyMappingUnitTest
    {
        [Test]
        public void NoName()
        {
            var xml = XElement.Parse("<property column='id' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTablePropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find name at table property mapping", ex.Message);
        }

        [Test]
        public void NoColumn()
        {
            var xml = XElement.Parse("<property name='Id' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTablePropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot find column at table property mapping", ex.Message);
        }

        [Test]
        public void NoInsert()
        {
            var xml = XElement.Parse("<property name='Id' column='id' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Insert);
        }

        [Test]
        public void NoUpdate()
        {
            var xml = XElement.Parse("<property name='Id' column='id' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Update);
        }

        [Test]
        public void NoConverter()
        {
            var xml = XElement.Parse("<property name='Id' column='id' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsNull(mapping.Converter);
        }

        [Test]
        public void NoGenerator()
        {
            var xml = XElement.Parse("<property name='Id' column='id' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsNull(mapping.Generator);
        }

        [Test]
        public void CheckInsertTrue()
        {
            var xml = XElement.Parse("<property name='Id' column='id' insert='true' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Insert);
        }

        [Test]
        public void CheckInsertFalse()
        {
            var xml = XElement.Parse("<property name='Id' column='id' insert='false'/>");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsFalse(mapping.Insert);
        }

        [Test]
        public void CheckInsert1()
        {
            var xml = XElement.Parse("<property name='Id' column='id' insert='1' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Insert);
        }

        [Test]
        public void CheckInsert0()
        {
            var xml = XElement.Parse("<property name='Id' column='id' insert='0'/>");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsFalse(mapping.Insert);
        }
        [Test]
        public void CheckUpdateTrue()
        {
            var xml = XElement.Parse("<property name='Id' column='id' update='true' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Update);
        }

        [Test]
        public void CheckUpdateFalse()
        {
            var xml = XElement.Parse("<property name='Id' column='id' update='false'/>");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsFalse(mapping.Update);
        }

        [Test]
        public void CheckUpdate1()
        {
            var xml = XElement.Parse("<property name='Id' column='id' update='1' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsTrue(mapping.Update);
        }

        [Test]
        public void CheckUpdate0()
        {
            var xml = XElement.Parse("<property name='Id' column='id' update='0'/>");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsFalse(mapping.Update);
        }

        [Test]
        public void CheckPublicFieldReference()
        {
            var xml = XElement.Parse("<property name='_version' column='version' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_version", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPrivateFieldReference()
        {
            var xml = XElement.Parse("<property name='_id' column='id' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("_id", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPublicPropertyReference()
        {
            var xml = XElement.Parse("<property name='Id' column='version' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("Id", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }

        [Test]
        public void CheckPrivatePropertyReference()
        {
            var xml = XElement.Parse("<property name='Version' column='version' />");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            var fieldInfo = typeof(Shape).GetMember("Version", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, mapping.Member);
        }


        [Test]
        public void CheckConverter()
        {
            var xml = XElement.Parse(string.Format("<property name='Version' column='version' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsInstanceOf<YesNoConverter>(mapping.Converter);
        }

        [Test]
        public void CheckPseudoConverter()
        {
            var pseudoConverterType = typeof(PseudoConverter).AssemblyQualifiedName;
            var iConverterType = typeof(IConverter).AssemblyQualifiedName;

            var xml = XElement.Parse(string.Format("<property name='Version' column='version' converter='{0}' />", pseudoConverterType));
            var ex = Assert.Throws<DocumentParseException>(() => new XmlTablePropertyMapping(typeof(Shape), xml));
            Assert.AreEqual(string.Format("Illegal converter class '{0}', class must be inherited from '{1}'", pseudoConverterType, iConverterType), ex.Message);
        }

        [Test]
        public void CheckWrongConverter()
        {
            var xml = XElement.Parse("<property name='Version' column='version' converter='WrongConverter' />");
            var ex = Assert.Throws<DocumentParseException>(() => new XmlTablePropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("Cannot parse converter type, unrecognized class 'WrongConverter'", ex.Message);
        }

        [Test]
        public void CheckEmptyGenerator()
        {
            var xml = XElement.Parse(@"
<property xmlns='urn:dbm-table-mapping' name='Version' column='version'>
  <generator />
</property>
");
            var ex = Assert.Throws<DocumentParseException>(() => new XmlTablePropertyMapping(typeof(Shape), xml));
            Assert.AreEqual("No generator type at table property mapping", ex.Message);
        }

        [Test]
        public void CheckGenerator()
        {
            var xml = XElement.Parse(@"
<property xmlns='urn:dbm-table-mapping' name='Version' column='version'>
  <generator>
    <db-assigned />
  </generator>
</property>
");
            var mapping = new XmlTablePropertyMapping(typeof(Shape), xml);
            Assert.IsInstanceOf<DbAssignedGenerator>(mapping.Generator);
        }      
    }
}