using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model;
using DbMapper.Mappings;
using Moq;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping
{
    [TestFixture]
    class ViewSubclassMappingUnitTest
    {
        [Test]
        public void NoName()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse("<subclass />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewSubClassMapping(mock.Object, xml));
            Assert.AreEqual("Cannot find name at view subclass mapping", ex.Message);
        }

        [Test]
        public void WrongName()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse("<subclass name='WrongClass' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewSubClassMapping(mock.Object, xml));
            Assert.AreEqual("Cannot recognize subclass 'WrongClass' at view subclass mapping", ex.Message);
        }

        [Test]
        public void CheckName()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format("<subclass name='{0}' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.AreEqual(typeof(TwoDimensionalShape), mapping.Type);
        }

        [Test]
        public void ClassDiscriminatorValueWithoutColumn()
        {
            var xml = XElement.Parse(string.Format("<subclass name='{0}' discriminator-value='str' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewSubClassMapping(null, xml));
            Assert.AreEqual("Cannot parse subclass discriminator value at view subclass mapping, unknown discriminator type", ex.Message);
        }

        [Test]
        public void WrongDiscriminatorValueType()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format("<subclass name='{0}' discriminator-value='str' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlViewSubClassMapping(mock.Object, xml));
            Assert.AreEqual(string.Format("Cannot parse subclass discriminator value 'str' as '{0}' at view subclass mapping", typeof(long).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void CheckDiscriminatorStringValue()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(string));

            var xml = XElement.Parse(string.Format("<subclass name='{0}' discriminator-value='str' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.AreEqual("str", mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckDiscriminatorLongValue()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format("<subclass name='{0}' discriminator-value='1' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.AreEqual(1L, mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckEmptyJoin()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format("<subclass name='{0}' />", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.IsNull(mapping.Join);
        }
        
        [Test]
        public void CheckJoin()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format(@"
<subclass xmlns='urn:dbm-view-mapping' name='{0}'>
  <join table='two_dimensional_shapes'>
    <column name='two_dimensional_shapes_id' join-column='id' />
  </join>
</subclass>", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);

            Assert.IsNotNull(mapping.Join);
            Assert.IsInstanceOf<XmlSubClassJoin>(mapping.Join);
        }

        [Test]
        public void CheckSubclasses()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format(@"
<subclass xmlns='urn:dbm-view-mapping' name='{0}'>
  <subclass name='{1}' />
  <subclass name='{2}' />
</subclass>", typeof(TwoDimensionalShape).AssemblyQualifiedName, typeof(Rectangle).AssemblyQualifiedName, typeof(Circle).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.AreEqual(2, mapping.SubClasses.Count);
        }

        [Test]
        public void CheckProperties()
        {
            var mock = new Mock<IDiscriminatorColumnMapping>();
            mock.Setup(c => c.Type).Returns(typeof(long));

            var xml = XElement.Parse(string.Format(@"
<subclass xmlns='urn:dbm-view-mapping' name='{0}'>
  <property name='X' column='x' />
  <property name='Y' column='y' />
</subclass>", typeof(TwoDimensionalShape).AssemblyQualifiedName));

            var mapping = new XmlViewSubClassMapping(mock.Object, xml);
            Assert.AreEqual(2, mapping.Properties.Count);
            Assert.IsInstanceOf<XmlViewPropertyMapping>(mapping.Properties[0]);
            Assert.IsInstanceOf<XmlViewPropertyMapping>(mapping.Properties[1]);
        }
    }
}