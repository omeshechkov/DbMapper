using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test
{
    [TestFixture]
    class SubclassJoinColumnMappingUnitTest
    {
        [Test]
        public void NoName()
        {
            var xml = XElement.Parse("<column join-schema='test_dbm' join-table='shapes' join-column='id' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlSubClassJoinColumn(xml));
            Assert.AreEqual("Cannot find name at subclass join column mapping", ex.Message);
        }

        [Test]
        public void NoJoinSchema()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-table='shapes' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.IsNull(mapping.JoinSchema);
        }

        [Test]
        public void NoJoinTable()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.IsNull(mapping.JoinTable);
        }

        [Test]
        public void NoJoinColumn()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-table='shapes' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlSubClassJoinColumn(xml));
            Assert.AreEqual("Cannot find join-column at subclass join column mapping", ex.Message);
        }

        [Test]
        public void CheckName()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-table='shapes' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.AreEqual("rectangle_id", mapping.Name);
        }

        [Test]
        public void CheckJoinSchema()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-table='shapes' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.AreEqual("test_dbm", mapping.JoinSchema);
        }

        [Test]
        public void CheckJoinTable()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-table='shapes' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.AreEqual("shapes", mapping.JoinTable);
        }

        [Test]
        public void CheckJoinColumn()
        {
            var xml = XElement.Parse("<column name='rectangle_id' join-schema='test_dbm' join-table='shapes' join-column='id' />");

            var mapping = new XmlSubClassJoinColumn(xml);
            Assert.AreEqual("id", mapping.JoinColumn);
        }
    }
}