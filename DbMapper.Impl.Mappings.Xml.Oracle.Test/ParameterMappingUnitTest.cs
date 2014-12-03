using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using DbMapper.Impl.Mappings.Xml.Oracle.Test.FunctionMappingUnitTest;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test
{
    [TestFixture]
    public class ParameterMappingUnitTest
    {
        private static readonly MethodInfo SuperFuncMethodInfo = typeof(DatabaseFacade.SuperFunc).GetMethod("Invoke");

        [Test]
        public void NoParameterName()
        {
            var xml = XElement.Parse("<parameter db-name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find name at function mapping", ex.Message);
        }

        [Test]
        public void NoParameterDbName()
        {
            var xml = XElement.Parse("<parameter name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find db-name at function mapping", ex.Message);
        }

        [Test]
        public void WrongParameterName()
        {
            var xml = XElement.Parse("<parameter name='Val' db-name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find parameter 'Val' at function parameter mapping", ex.Message);
        }

        [Test]
        public void CheckParameterName()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            var parameterInfo = SuperFuncMethodInfo.GetParameters().First(p => p.Name == "val");
            Assert.AreEqual(parameterInfo, mapping.Parameter);
        }

        [Test]
        public void CheckParameterDbName()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual("val", mapping.DbParameterName);
        }

        [Test]
        public void NoParameterConverter()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.IsNull(mapping.Converter);
        }

        [Test]
        public void CheckParameterConverter()
        {
            var xml = XElement.Parse(string.Format("<parameter name='val' db-name='val' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.IsInstanceOf<YesNoConverter>(mapping.Converter);
        }

        [Test]
        public void CheckParameterPseudoConverter()
        {
            var pseudoConverterType = typeof(PseudoConverter).AssemblyQualifiedName;
            var iConverterType = typeof(IConverter).AssemblyQualifiedName;
            var xml = XElement.Parse(string.Format("<parameter name='val' db-name='val' converter='{0}' />", pseudoConverterType));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual(string.Format("Illegal converter class '{0}', class must be inherited from '{1}'", pseudoConverterType, iConverterType), ex.Message);
        }

        [Test]
        public void NoParameterDbType()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual((DbType)(-1), mapping.DbType);
        }

        [Test]
        public void CheckParameterDbType()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' db-type='Int64' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(DbType.Int64, mapping.DbType);
        }        
        
        [Test]
        public void NoParameterLength()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(0, mapping.Length);
        }

        [Test]
        public void CheckParameterLength()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' length='123' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(123, mapping.Length);
        }        
    }
}