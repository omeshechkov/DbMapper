using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableSubclassJoinMappingUnitTest
    {

        [Test]
        public void NoTable()
        {
            var xml = XElement.Parse(@"
<join schema='test_dbm'>
  <column name='rectangle_id' join-schema='test_dbm' join-table='shape' join-column='id'/>
</join>");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlSubClassJoin(xml));
            Assert.AreEqual("Cannot find table at table subclass join mapping", ex.Message);
        }

        [Test]
        public void CheckTable()
        {
            var xml = XElement.Parse(@"
<join schema='test_dbm' table='rectangles'>
  <column name='rectangle_id' join-schema='test_dbm' join-table='shape' join-column='id'/>
</join>");

            var mapping = new XmlSubClassJoin(xml);
            Assert.AreEqual("rectangles", mapping.Table);
        }

        [Test]
        public void NoSchema()
        {
            var xml = XElement.Parse(@"
<join table='rectangles'>
  <column name='rectangle_id' join-schema='test_dbm' join-table='shape' join-column='id'/>
</join>");

            var mapping = new XmlSubClassJoin(xml);
            Assert.IsNull(mapping.Schema);
        }

        [Test]
        public void CheckSchema()
        {
            var xml = XElement.Parse(@"
<join schema='test_dbm' table='rectangles'>
  <column name='rectangle_id' join-schema='test_dbm' join-table='shape' join-column='id'/>
</join>");

            var mapping = new XmlSubClassJoin(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }
        
        [Test]
        public void CheckColumns()
        {
            var xml = XElement.Parse(@"
<join xmlns='urn:dbm-table-mapping' schema='test_dbm' table='rectangles'>
  <column name='rectangle_id' join-schema='test_dbm' join-table='shape' join-column='id'/>
</join>");

            var mapping = new XmlSubClassJoin(xml);
            Assert.AreEqual(1, mapping.ColumnJoins.Count);
        }
    }
}