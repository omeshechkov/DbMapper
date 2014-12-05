using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping
{
    [TestFixture]
    class ViewMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(null));
            Assert.AreEqual("Cannot build view mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoTableMapping()
        {
            var xml = XElement.Parse("<view-mapping xmlns='urn:dbm-view-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(xml));
            Assert.AreEqual("Cannot find view at view mapping", ex.Message);
        }

        [Test]
        public void NoProperties()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}' />
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual(0, mapping.Properties.Count);
        }

        [Test]
        public void NoName()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(xml));
            Assert.AreEqual("Cannot find name at view mapping", ex.Message);
        }

        [Test]
        public void NoClass()
        {
            var xml = XElement.Parse(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes'>
    <property name='Id' column='id' />
  </view>
</view-mapping>");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(xml));
            Assert.AreEqual("Cannot find class at view mapping", ex.Message);
        }

        [Test]
        public void NoSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.IsNull(mapping.Schema);
        }

        [Test]
        public void NoDiscriminatorValue()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.IsNull(mapping.DiscriminatorValue);
        }

        [Test]
        public void NoDiscriminator()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.IsNull(mapping.Discriminator);
        }

        [Test]
        public void CheckSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }

        [Test]
        public void CheckName()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual("shapes", mapping.Name);
        }

        [Test]
        public void CheckClass()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual(typeof(Shape), mapping.Type);
        }

        [Test]
        public void ClassDiscriminatorValueWithoutColumn()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}' discriminator-value='1'>
    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(xml));
            Assert.AreEqual("Cannot parse view discriminator value, unknown discriminator type", ex.Message);
        }

        [Test]
        public void WrongDiscriminatorValueType()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}' discriminator-value='Shape'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewMapping(xml));
            Assert.AreEqual(string.Format("Cannot parse view discriminator value 'Shape' as '{0}'", typeof(long).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void CheckDiscriminatorStringValue()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}' discriminator-value='Shape'>
    <discriminator column='type' type='string' />

    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual("Shape", mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckDiscriminatorLongValue()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}' discriminator-value='123'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual(123L, mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckProperties()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <property name='Id' column='id' />
    
    <property name='Version' column='version' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual(2L, mapping.Properties.Count);
            Assert.IsInstanceOf<XmlViewPropertyMapping>(mapping.Properties[0]);
            Assert.IsInstanceOf<XmlViewPropertyMapping>(mapping.Properties[1]);
        }

        [Test]
        public void CheckSubclasses()
        {
            var xml = XElement.Parse(string.Format(@"
<view-mapping xmlns='urn:dbm-view-mapping'>
  <view schema='test_dbm' name='shapes' class='{0}'>
    <discriminator column='type' type='long' />

    <property name='Id' column='id' />
    
    <subclass name='{1}' discriminator-value='1' />
    <subclass name='{2}' discriminator-value='2' />
  </view>
</view-mapping>", typeof(Shape).AssemblyQualifiedName, typeof(TwoDimensionalShape).AssemblyQualifiedName, typeof(ThreeDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewMapping(xml);
            Assert.AreEqual(2, mapping.SubClasses.Count);
        }
    }
}