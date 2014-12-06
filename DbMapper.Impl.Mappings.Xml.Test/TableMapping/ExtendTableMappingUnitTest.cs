using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class ExtendTableMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendTableMapping(null));
            Assert.AreEqual("Cannot build extend-table mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoExtendTableMapping()
        {
            var xml = XElement.Parse("<extend-table-mapping xmlns='urn:dbm-extend-table-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendTableMapping(xml));
            Assert.AreEqual("Cannot find extend-table at extend-table mapping", ex.Message);
        }

        [Test]
        public void NoClass()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-table-mapping xmlns='urn:dbm-extend-table-mapping'>
  <extend-table>
    <discriminator column='type' type='long' />

    <subclass name='{0}' discriminator-value='1' />
  </extend-table>
</extend-table-mapping>" , typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendTableMapping(xml));
            Assert.AreEqual("Cannot find class at extend-table mapping", ex.Message);
        }

        [Test]
        public void NoDiscriminator()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-table-mapping xmlns='urn:dbm-extend-table-mapping'>
  <extend-table class='{0}'>
    <subclass name='{1}' discriminator-value='1' />
  </extend-table>
</extend-table-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendTableMapping(xml));
            Assert.AreEqual("Cannot find discriminator at extend-table mapping", ex.Message);
        }

        [Test]
        public void CheckClass()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-table-mapping xmlns='urn:dbm-extend-table-mapping'>
  <extend-table class='{0}'>
    <discriminator column='type' type='long' />

    <subclass name='{1}' discriminator-value='1' />
  </extend-table>
</extend-table-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlExtendTableMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.Type);
        }        
        
        [Test]
        public void CheckSubclasses()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-table-mapping xmlns='urn:dbm-extend-table-mapping'>
  <extend-table class='{0}'>
    <discriminator column='type' type='long' />
    
    <subclass name='{1}' discriminator-value='1' />
    <subclass name='{2}' discriminator-value='2' />
  </extend-table>
</extend-table-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName, typeof(ThreeDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlExtendTableMapping(xml);
            Assert.AreEqual(2, mapping.SubClasses.Count);
        }
    }
}