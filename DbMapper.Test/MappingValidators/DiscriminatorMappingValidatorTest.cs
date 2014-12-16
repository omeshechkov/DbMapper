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
    public class DiscriminatorMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => discriminatorMappingValidator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => discriminatorMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Discriminator mapping validation error, mapping '{0}' is not a discriminator column mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullColumn()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => discriminatorMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Discriminator mapping validation error, column is null or empty", ex.Message);
        }

        [Test]
        public void EmptyColumn()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();
            mappingMock.Setup(m => m.Column).Returns(string.Empty);
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => discriminatorMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Discriminator mapping validation error, column is null or empty", ex.Message);
        }

        [Test]
        public void NullType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();
            mappingMock.Setup(m => m.Column).Returns("type");
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => discriminatorMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Discriminator mapping validation error, type is null", ex.Message);
        }

        [Test]
        public void UnsupportedType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();
            mappingMock.Setup(m => m.Column).Returns("type");
            mappingMock.Setup(m => m.Type).Returns(typeof(bool));
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => discriminatorMappingValidator.Validate(mappingMock.Object, null));

            const string supportedTypes = "System.String, System.Type, System.Byte, System.Int16, System.Int32, System.Int64, System.Single, System.Double, System.Decimal, System.Guid";

            Assert.AreEqual(string.Format("Discriminator mapping validation error, type '{0}' is not supported, supported types: [{1}]", typeof(bool).AssemblyQualifiedName, supportedTypes), ex.Message);
        }
        
        [Test]
        public void CorrectMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();
            mappingMock.Setup(m => m.Column).Returns("type");
            mappingMock.Setup(m => m.Type).Returns(typeof(long));
            
            var discriminatorMappingValidator = new DiscriminatorMappingValidator(factoryMock.Object);
            discriminatorMappingValidator.Validate(mappingMock.Object, null);
        }
    }
}