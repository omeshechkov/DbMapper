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
        public void NoName()
        {
            var xml = XElement.Parse("<parameter db-name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find name at function mapping", ex.Message);
        }

        [Test]
        public void NoDbName()
        {
            var xml = XElement.Parse("<parameter name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find db-name at function mapping", ex.Message);
        }

        [Test]
        public void WrongName()
        {
            var xml = XElement.Parse("<parameter name='Val' db-name='val' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlParameterMapping(SuperFuncMethodInfo, xml));
            Assert.AreEqual("Cannot find parameter 'Val' at function parameter mapping", ex.Message);
        }

        [Test]
        public void CheckName()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            var parameterInfo = SuperFuncMethodInfo.GetParameters().First(p => p.Name == "val");
            Assert.AreEqual(parameterInfo, mapping.Parameter);
        }

        [Test]
        public void CheckDbName()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual("val", mapping.DbParameterName);
        }

        [Test]
        public void NoConverter()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.IsNull(mapping.Converter);
        }

        [Test]
        public void CheckConverter()
        {
            var xml = XElement.Parse(string.Format("<parameter name='val' db-name='val' converter='{0}' />", typeof(YesNoConverter).AssemblyQualifiedName));

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.IsInstanceOf<YesNoConverter>(mapping.Converter);
        }

        [Test]
        public void NoDbType()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual((DbType)(-1), mapping.DbType);
        }

        [Test]
        public void CheckDbType()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' db-type='Int64' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(DbType.Int64, mapping.DbType);
        }        
        
        [Test]
        public void NoLength()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(0, mapping.Length);
        }

        [Test]
        public void CheckLength()
        {
            var xml = XElement.Parse("<parameter name='val' db-name='val' length='123' />");

            var mapping = new XmlParameterMapping(SuperFuncMethodInfo, xml);
            Assert.AreEqual(123, mapping.Length);
        }        
    }
}