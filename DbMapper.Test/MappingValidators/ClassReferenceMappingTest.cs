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
    public class MappingClassReferenceTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var validator = new MappingClassReferenceValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var validator = new MappingClassReferenceValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Mapping class reference validation error, mapping '{0}' is not a mapping class reference", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IMappingClassReference>();

            var validator = new MappingClassReferenceValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Mapping class reference validation error, type is null", ex.Message);
        }

        [Test]
        public void CorrectMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IMappingClassReference>();
            mappingMock.Setup(m => m.Type).Returns(typeof(object));

            var validator = new MappingClassReferenceValidator(factoryMock.Object);
            validator.Validate(mappingMock.Object, null);
        }
    }
}