using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping
{
    [TestFixture]
    class ExtendViewMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendViewMapping(null));
            Assert.AreEqual("Cannot build extend-view mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoExtendTableMapping()
        {
            var xml = XElement.Parse("<extend-view-mapping xmlns='urn:dbm-extend-view-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendViewMapping(xml));
            Assert.AreEqual("Cannot find extend-view at extend-view mapping", ex.Message);
        }

        [Test]
        public void NoClass()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-view-mapping xmlns='urn:dbm-extend-view-mapping'>
  <extend-view>
    <discriminator column='type' type='long' />

    <subclass name='{0}' discriminator-value='1' />
  </extend-view>
</extend-view-mapping>" , typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendViewMapping(xml));
            Assert.AreEqual("Cannot find class at extend-view mapping", ex.Message);
        }

        [Test]
        public void NoDiscriminator()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-view-mapping xmlns='urn:dbm-extend-view-mapping'>
  <extend-view class='{0}'>
    <subclass name='{1}' discriminator-value='1' />
  </extend-view>
</extend-view-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlExtendViewMapping(xml));
            Assert.AreEqual("Cannot find discriminator at extend-view mapping", ex.Message);
        }

        [Test]
        public void CheckClass()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-view-mapping xmlns='urn:dbm-extend-view-mapping'>
  <extend-view class='{0}'>
    <discriminator column='type' type='long' />

    <subclass name='{1}' discriminator-value='1' />
  </extend-view>
</extend-view-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlExtendViewMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.Type);
        }        
        
        [Test]
        public void CheckSubclasses()
        {
            var xml = XElement.Parse(string.Format(@"
<extend-view-mapping xmlns='urn:dbm-extend-view-mapping'>
  <extend-view class='{0}'>
    <discriminator column='type' type='long' />
    
    <subclass name='{1}' discriminator-value='1' />
    <subclass name='{2}' discriminator-value='2' />
  </extend-view>
</extend-view-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName, typeof(ThreeDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlExtendViewMapping(xml);
            Assert.AreEqual(2, mapping.SubClasses.Count);
        }
    }
}