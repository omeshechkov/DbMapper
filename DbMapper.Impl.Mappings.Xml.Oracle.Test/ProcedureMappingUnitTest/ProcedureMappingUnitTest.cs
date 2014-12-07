using System;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Oracle.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Test.ProcedureMappingUnitTest
{
    [TestFixture]
    public class ProcedureMappingUnitTest
    {
        [Test]
        public void NullInput()
        {
            var ex = Assert.Throws<DocumentParseException>(() => new XmlProcedureMapping(null));
            Assert.AreEqual("Cannot build procedure mapping", ex.Message);
            Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
        }   

        [Test]
        public void NoMapping()
        {
            var xml = XElement.Parse("<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping' />");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlProcedureMapping(xml));
            Assert.AreEqual("Cannot find procedure at procedure mapping", ex.Message);
        }   
        
        [Test]
        public void NoSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure name='management.super_proc' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.IsNull(mapping.Schema);
        }  
        
        [Test]
        public void NoName()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var ex = Assert.Throws<DocumentParseException>(() => new XmlProcedureMapping(xml));
            Assert.AreEqual("Cannot find name at procedure mapping", ex.Message);
        }       

        [Test]
        public void NoDelegate()
        {
            var xml = XElement.Parse(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' />
</procedure-mapping>");

            var ex = Assert.Throws<DocumentParseException>(() => new XmlProcedureMapping(xml));
            Assert.AreEqual("Cannot find delegate at procedure mapping", ex.Message);
        }       

        [Test]
        public void CheckSchema()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.AreEqual("test_dbm", mapping.Schema);
        }
        
        [Test]
        public void CheckName()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.AreEqual("management.super_proc", mapping.Name);
        }

        [Test]
        public void CheckDelegate()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.AreEqual(typeof(DatabaseFacade.SuperProc), mapping.Type);
            Assert.AreEqual(typeof(DatabaseFacade.SuperProc).GetMethod("Invoke"), mapping.Delegate);
        }
        
        [Test]
        public void NoParameters()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' delegate='{0}' />
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.AreEqual(0, mapping.Parameters.Count);
        } 
        
        [Test]
        public void CheckParameters()
        {
            var xml = XElement.Parse(string.Format(@"
<procedure-mapping xmlns='urn:dbm-oracle-procedure-mapping'>
  <procedure schema='test_dbm' name='management.super_proc' delegate='{0}'>
    <parameter name='val' db-name='val' />
    
    <parameter name='str' db-name='str' />
  </procedure>
</procedure-mapping>", typeof(DatabaseFacade.SuperProc).AssemblyQualifiedName));

            var mapping = new XmlProcedureMapping(xml);
            Assert.AreEqual(2, mapping.Parameters.Count);
            Assert.IsInstanceOf<XmlParameterMapping>(mapping.Parameters[0]);
            Assert.IsInstanceOf<XmlParameterMapping>(mapping.Parameters[1]);
        }       
    }
}