using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test.FunctionMappingUnitTest
{
    [TestFixture]
    public class FunctionMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlFunctionMapping(null));
            Assert.AreEqual("Cannot build function mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }

        [Test]
        public void NoFunctionMapping()
        {
            var xml = XElement.Parse("<function-mapping xmlns='urn:dbm-oracle-function-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlFunctionMapping(xml));
            Assert.AreEqual("Cannot find function at function mapping", ex.Message);
        }

        [Test]
        public void NoFunctionSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.IsNull(mapping.Schema);
        }  
        
        [Test]
        public void NoFunctionName()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlFunctionMapping(xml));
            Assert.AreEqual("Cannot find name at function mapping", ex.Message);
        }       

        [Test]
        public void NoFunctionDelegate()
        {
            var xml = XElement.Parse(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func'>
    <return-value length='123' />
  </function>
</function-mapping>");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlFunctionMapping(xml));
            Assert.AreEqual("Cannot find delegate at function mapping", ex.Message);
        }       

        [Test]
        public void CheckFunctionSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }
        
        [Test]
        public void CheckFunctionName()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.AreEqual("management.super_func", mapping.Name);
        }

        [Test]
        public void CheckFunctionDelegate()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.AreEqual(typeof(DatabaseFacade.SuperFunc), mapping.Type);
            Assert.AreEqual(typeof(DatabaseFacade.SuperFunc).GetMethod("Invoke"), mapping.Delegate);
        }
        
        [Test]
        public void CheckFunctionNoParameters()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.AreEqual(0, mapping.Parameters.Count);
        } 
        
        [Test]
        public void CheckFunctionParameters()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />

    <parameter name='val' db-name='val' />
    
    <parameter name='str' db-name='str' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.AreEqual(2, mapping.Parameters.Count);
        }

        [Test]
        public void CheckFunctionReturnValue()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}'>
    <return-value length='123' />
  </function>
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var mapping = new XmlFunctionMapping(xml);
            Assert.IsInstanceOf<XmlFunctionReturnValueMapping>(mapping.ReturnValue);
        } 
        
        [Test]
        public void CheckFunctionNoReturnValue()
        {
            var xml = XElement.Parse(string.Format(@"
<function-mapping xmlns='urn:dbm-oracle-function-mapping'>
  <function schema='test_dbm' name='management.super_func' delegate='{0}' />
</function-mapping>", typeof(DatabaseFacade.SuperFunc).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlFunctionMapping(xml));
            Assert.AreEqual("Cannot find return-value at function mapping", ex.Message);
        }
    }
}