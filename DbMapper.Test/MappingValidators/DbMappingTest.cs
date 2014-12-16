using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
{
    [TestFixture]
    public class DbMappingTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var validator = new DbMappingValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var validator = new DbMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("DB-mapping validation error, mapping '{0}' is not a db-mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDbMapping>();

            var validator = new DbMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual("DB-mapping validation error, name is null or empty", ex.Message);
        }
 
        [Test]
        public void EmptyName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDbMapping>();
            mappingMock.Setup(m => m.Name).Returns(string.Empty);

            var validator = new DbMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual("DB-mapping validation error, name is null or empty", ex.Message);
        }

        [Test]
        public void NullSchema()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDbMapping>();
            mappingMock.Setup(m => m.Name).Returns("table");

            var validator = new DbMappingValidator(factoryMock.Object);
            validator.Validate(mappingMock.Object, null);
        }

        [Test]
        public void EmptySchema()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDbMapping>();
            mappingMock.Setup(m => m.Name).Returns("table");
            mappingMock.Setup(m => m.Schema).Returns(string.Empty);

            var validator = new DbMappingValidator(factoryMock.Object);
            validator.Validate(mappingMock.Object, null);
        }

        [Test]
        public void CorrectMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDbMapping>();
            mappingMock.Setup(m => m.Name).Returns("table");
            mappingMock.Setup(m => m.Schema).Returns("schema");

            var validator = new DbMappingValidator(factoryMock.Object);
            validator.Validate(mappingMock.Object, null);
        }
    }
}