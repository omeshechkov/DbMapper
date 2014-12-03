using System;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test
{
    [TestFixture]
    public class ParseTypeUnitTest
    {
        [Test]
        public void ResolveValueCLRTypeAsNullable()
        {
            Assert.AreEqual(TypeUtils.ParseType("System.DateTime?", false), typeof(DateTime?));
        }   

        [Test]
        [ExpectedException(typeof(ParseTypeException))]
        public void ResolveNonValueCLRTypeAsNullable()
        {
            Assert.AreEqual(TypeUtils.ParseType("System.Object?", false), typeof(object));
        }   

        [Test]
        public void ResolveCLRType()
        {
            Assert.AreEqual(TypeUtils.ParseType("System.Object", false), typeof(object));
        }   

        [Test]
        [ExpectedException(typeof(ParseTypeException))]
        public void ResolveCLRTypeOnlyShorthands()
        {
            Assert.AreEqual(TypeUtils.ParseType("System.Object", true), typeof(bool));
        }   
        
        [Test]
        [ExpectedException(typeof(ParseTypeException))]
        public void ResolveWrongType()
        {
            Assert.AreEqual(TypeUtils.ParseType("wrong-type", true), typeof(bool));
        }

        [Test]
        public void ResolveBooleanType()
        {
            Assert.AreEqual(TypeUtils.ParseType("bool", true), typeof(bool));
        }

        [Test]
        public void ResolveNullableBooleanType()
        {
            Assert.AreEqual(TypeUtils.ParseType("bool?", true), typeof(bool?));
        }

        [Test]
        public void ResolveStringType()
        {
            Assert.AreEqual(TypeUtils.ParseType("string", true), typeof(string));
        }

        [Test]
        [ExpectedException(typeof(ParseTypeException))]
        public void ResolveNullableStringType()
        {
            Assert.AreEqual(TypeUtils.ParseType("string?", true), typeof(string));
        }

        [Test]
        public void ResolveTypeType()
        {
            Assert.AreEqual(TypeUtils.ParseType("type", true), typeof(Type));
        }

        [Test]
        [ExpectedException(typeof(ParseTypeException))]
        public void ResolveNullableTypeType()
        {
            Assert.AreEqual(TypeUtils.ParseType("type?", true), typeof(Type));
        }

        [Test]
        public void ResolveDateTimeType()
        {
            Assert.AreEqual(TypeUtils.ParseType("date-time", true), typeof(DateTime));
        }

        [Test]
        public void ResolveByteType()
        {
            Assert.AreEqual(TypeUtils.ParseType("byte", true), typeof(byte));
        }

        [Test]
        public void ResolveNullableByteType()
        {
            Assert.AreEqual(TypeUtils.ParseType("byte?", true), typeof(byte?));
        }

        [Test]
        public void ResolveShortType()
        {
            Assert.AreEqual(TypeUtils.ParseType("short", true), typeof(short));
        }

        [Test]
        public void ResolveNullableShortType()
        {
            Assert.AreEqual(TypeUtils.ParseType("short?", true), typeof(short?));
        }

        [Test]
        public void ResolveIntType()
        {
            Assert.AreEqual(TypeUtils.ParseType("int", true), typeof(int));
        }

        [Test]
        public void ResolveNullableIntType()
        {
            Assert.AreEqual(TypeUtils.ParseType("int?", true), typeof(int?));
        }

        [Test]
        public void ResolveLongType()
        {
            Assert.AreEqual(TypeUtils.ParseType("long", true), typeof(long));
        }

        [Test]
        public void ResolveNullableLongType()
        {
            Assert.AreEqual(TypeUtils.ParseType("long?", true), typeof(long?));
        }

        [Test]
        public void ResolveFloatType()
        {
            Assert.AreEqual(TypeUtils.ParseType("float", true), typeof(float));
        }

        [Test]
        public void ResolveNullableFloatType()
        {
            Assert.AreEqual(TypeUtils.ParseType("float?", true), typeof(float?));
        } 
        
        [Test]
        public void ResolveDoubleType()
        {
            Assert.AreEqual(TypeUtils.ParseType("double", true), typeof(double));
        }

        [Test]
        public void ResolveNullableDoubleType()
        {
            Assert.AreEqual(TypeUtils.ParseType("double?", true), typeof(double?));
        }

        [Test]
        public void ResolveDecimalType()
        {
            Assert.AreEqual(TypeUtils.ParseType("decimal", true), typeof(decimal));
        }

        [Test]
        public void ResolveNullableDecimalType()
        {
            Assert.AreEqual(TypeUtils.ParseType("decimal?", true), typeof(decimal?));
        }

        [Test]
        public void ResolveGuidType()
        {
            Assert.AreEqual(TypeUtils.ParseType("guid", true), typeof(Guid));
        }

        [Test]
        public void ResolveNullableGuidType()
        {
            Assert.AreEqual(TypeUtils.ParseType("guid?", true), typeof(Guid?));
        }
    }
}
