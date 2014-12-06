using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(null));
            Assert.AreEqual("Cannot build table mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoTableMapping()
        {
            var xml = XElement.Parse("<table-mapping xmlns='urn:dbm-table-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot find table at table mapping", ex.Message);
        }

        [Test]
        public void NoProperties()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}' />
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual(0, mapping.Properties.Count);
        }

        [Test]
        public void NoName()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot find name at table mapping", ex.Message);
        }

        [Test]
        public void NoClass()
        {
            var xml = XElement.Parse(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes'>
    <property name='Id' column='id' />
  </table>
</table-mapping>");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot find class at table mapping", ex.Message);
        }

        [Test]
        public void NoSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.IsNull(mapping.Schema);
        }

        [Test]
        public void NoDiscriminatorValue()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.IsNull(mapping.DiscriminatorValue);
        }

        [Test]
        public void NoDiscriminator()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.IsNull(mapping.Discriminator);
        }

        [Test]
        public void CheckSchemaName()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);            
            Assert.AreEqual("test_dbm", mapping.Schema);
        }

        [Test]
        public void CheckName()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual("shapes", mapping.Name);
        }

        [Test]
        public void CheckClass()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.Type);
        }

        [Test]
        public void ClassDiscriminatorValueWithoutColumn()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}' discriminator-value='1'>
    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot parse table discriminator value, unknown discriminator type", ex.Message);
        }

        [Test]
        public void WrongDiscriminatorValueType()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}' discriminator-value='Shape'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual(string.Format("Cannot parse table discriminator value 'Shape' as '{0}'", typeof(long).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void CheckDiscriminatorStringValue()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}' discriminator-value='Shape'>
    <discriminator column='type' type='string' />

    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual("Shape", mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckDiscriminatorLongValue()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}' discriminator-value='123'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual(123L, mapping.DiscriminatorValue);
        }

        [Test]
        public void NoPrimaryKeyPropertyName()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <property name='TimeAndZone' column='time_and_zone' />

    <primary-key>
      <property />
    </primary-key>
  </table>
</table-mapping>", typeof(PrimaryKeyMemberReference).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot find name at table primary key mapping", ex.Message);
        }

        [Test]
        public void WrongPrimaryKeyPropertyName()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <property name='TimeAndZone' column='time_and_zone' />

    <primary-key>
      <property name='Idd' />
    </primary-key>
  </table>
</table-mapping>", typeof(PrimaryKeyMemberReference).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlTableMapping(xml));
            Assert.AreEqual("Cannot find primary key property 'Idd' at table primary key mapping", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPrimaryKeyPropertyReference()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <property name='TimeAndZone' column='time_and_zone' />

    <primary-key>
      <property name='Id' />
      <property name='TimeAndZone' />
    </primary-key>
  </table>
</table-mapping>", typeof(PrimaryKeyMemberReference).AssemblyQualifiedName));


            var mapping = new XmlTableMapping(xml);
            var primaryKeyProperties = mapping.PrimaryKeyProperties.ToArray();
            var idPropertyInfo = typeof(PrimaryKeyMemberReference).GetMember("Id", BindingFlags.Instance | BindingFlags.Public).First();
            var timeAndZoneFieldInfo = typeof(PrimaryKeyMemberReference).GetMember("TimeAndZone", BindingFlags.Instance | BindingFlags.Public).First();
            
            Assert.AreEqual(2, primaryKeyProperties.Length);

            Assert.Contains(idPropertyInfo, primaryKeyProperties);
            Assert.Contains(timeAndZoneFieldInfo, primaryKeyProperties);
        }

        [Test]
        public void CheckProperties()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <property name='Version' column='version' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual(2L, mapping.Properties.Count);
            Assert.IsInstanceOf<XmlTablePropertyMapping>(mapping.Properties[0]);
            Assert.IsInstanceOf<XmlTablePropertyMapping>(mapping.Properties[1]);
        }
        
        [Test]
        public void CheckVersion()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <version name='Version' column='version' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.IsInstanceOf<XmlVersionPropertyMapping>(mapping.Version);
        }
        
        [Test]
        public void CheckSubclasses()
        {
            var xml = XElement.Parse(string.Format(@"
<table-mapping xmlns='urn:dbm-table-mapping'>
  <table schema='test_dbm' name='shapes' class='{0}'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
    
    <subclass name='{1}' discriminator-value='1' />
    <subclass name='{2}' discriminator-value='2' />
  </table>
</table-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName, typeof(ThreeDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlTableMapping(xml);
            Assert.AreEqual(2, mapping.SubClasses.Count);
        }
    }
}