using System.Xml.Linq;
using DbMapper.Generators;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TablePropertyGeneratorFactoryUnitTest
    {

        [Test]
        public void CheckWrongGenerator()
        {
            var xml = XElement.Parse("<wrong-generator xmlns='urn:dbm-table-mapping' />");
            var ex = Assert.Throws<DocumentParseException>(() => GeneratorFactory.GetGenerator(xml));
            Assert.AreEqual("Unknown generator type '{urn:dbm-table-mapping}wrong-generator'", ex.Message);
        }

        [Test]
        public void CheckPropertySequenceGenerator()
        {
            var xml = XElement.Parse("<sequence xmlns='urn:dbm-table-mapping' name='seq_shapes' />");
            var generator = GeneratorFactory.GetGenerator(xml);

            Assert.IsInstanceOf<SequenceGenerator>(generator);
            var sequenceGenerator = (SequenceGenerator)generator;
            Assert.AreEqual(sequenceGenerator.Name, "seq_shapes");
        }


        [Test]
        public void NoSequenceGeneratorName()
        {
            var xml = XElement.Parse("<sequence xmlns='urn:dbm-table-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => GeneratorFactory.GetGenerator(xml));
            Assert.AreEqual("Cannot find name at sequence generator", ex.Message);
        }
    }
}