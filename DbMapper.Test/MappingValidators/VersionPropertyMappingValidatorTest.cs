using System.Linq;
using System.Reflection;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using DbMapper.Test.MappingValidators.Model;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
{
    [TestFixture]
    public class VersionVersionPropertyMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => versionVersionPropertyMappingValidator.Validate(null, null));
            Assert.AreEqual("Version property mapping validation error, mapping is null", ex.Message);
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => versionVersionPropertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Version property mapping validation error, mapping '{0}' is not a property mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullMember()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IVersionPropertyMapping>();

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => versionVersionPropertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Version property mapping validation error, member is null", ex.Message);
        }

        [Test]
        public void NotAPropertyOrField()
        {
            var methodInfo = typeof(Model.Entity).GetMember("GetValue", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IVersionPropertyMapping>();
            mappingMock.Setup(m => m.Member).Returns(methodInfo);

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => versionVersionPropertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Version property mapping validation error, member is not a property or a field", ex.Message);
        }

        [Test]
        public void UnsupportedType()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("Value", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IVersionPropertyMapping>();
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => versionVersionPropertyMappingValidator.Validate(mappingMock.Object, null));

            const string supportedTypes = "System.Byte, System.Int16, System.Int32, System.Int64, System.Single, System.Double, System.Decimal";

            Assert.AreEqual(string.Format("Version property mapping validation error, type '{0}' is not supported, supported types: [{1}]", typeof(object).AssemblyQualifiedName, supportedTypes), ex.Message);
        }

        [Test]
        public void CorrectMapping()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("Id", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IVersionPropertyMapping>();
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var versionVersionPropertyMappingValidator = new VersionPropertyMappingValidator(factoryMock.Object);
            versionVersionPropertyMappingValidator.Validate(mappingMock.Object, null);
        }
    }
}