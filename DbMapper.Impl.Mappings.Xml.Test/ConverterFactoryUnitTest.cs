using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Test.Converters;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test
{
    [TestFixture]
    public class ConverterFactoryUnitTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ConverterFactory.RegisterShorthand<YesNoConverter>("YN");
        }

        [Test]
        public void ResolveShorthand()
        {
            Assert.IsInstanceOf<YesNoConverter>(ConverterFactory.Create("YN"));
        }

        [Test]
        public void ResolveYesNoConverter()
        {
            Assert.IsInstanceOf<YesNoConverter>(ConverterFactory.Create(typeof(YesNoConverter).AssemblyQualifiedName));
        }

        [Test]
        public void CheckWrongConverter()
        {
            var ex = Assert.Throws<DocumentParseException>(() => ConverterFactory.Create("WrongConverter"));
            Assert.AreEqual("Cannot parse converter type, unrecognized class 'WrongConverter'", ex.Message);
        }

        [Test]
        public void CheckPseudoConverter()
        {
            var pseudoConverterType = typeof(PseudoConverter).AssemblyQualifiedName;
            var iConverterType = typeof(IConverter).AssemblyQualifiedName;

            var ex = Assert.Throws<DocumentParseException>(() => ConverterFactory.Create(pseudoConverterType));
            Assert.AreEqual(string.Format("Illegal converter class '{0}', class must be inherited from '{1}'", pseudoConverterType, iConverterType), ex.Message);
        }
    }
}
