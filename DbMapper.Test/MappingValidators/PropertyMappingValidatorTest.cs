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
    public class PropertyMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(null, null));
            Assert.AreEqual("Property mapping validation error, mapping is null", ex.Message);
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IDiscriminatorMapping>();

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Property mapping validation error, mapping '{0}' is not a property mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property mapping validation error, column is null or empty", ex.Message);
        }

        [Test]
        public void EmptyName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns(string.Empty);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property mapping validation error, column is null or empty", ex.Message);
        }

        [Test]
        public void NullMember()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property 'id' mapping validation error, member is null", ex.Message);
        }

        [Test]
        public void NotAPropertyOrField()
        {
            var methodInfo = typeof(Model.Entity).GetMember("GetValue", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");
            mappingMock.Setup(m => m.Member).Returns(methodInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property 'id' mapping validation error, member is not a property or a field", ex.Message);
        }

        [Test]
        public void StaticMember()
        {
            var staticPropertyInfo = typeof(Model.Entity).GetMember("StaticField", BindingFlags.Public | BindingFlags.Static).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");
            mappingMock.Setup(m => m.Member).Returns(staticPropertyInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property 'id' mapping validation error, property is static", ex.Message);
        }

        [Test]
        public void NoGetter()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("NoGetter", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property 'id' mapping validation error, no getter", ex.Message);
        }

        [Test]
        public void NoSetter()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("NoSetter", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Property 'id' mapping validation error, no setter", ex.Message);
        }

        [Test]
        public void UnsupportedType()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("Value", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("value");
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => propertyMappingValidator.Validate(mappingMock.Object, null));

            const string supportedTypes = "System.Boolean, System.String, System.Type, System.Byte, System.Int16, System.Int32, System.Int64, System.Single, System.Double, System.Decimal, System.Guid, System.DateTime, " +
                                          "System.Nullable`1[System.Boolean], System.Nullable`1[System.Byte], System.Nullable`1[System.Int16], System.Nullable`1[System.Int32], System.Nullable`1[System.Int64], " +
                                          "System.Nullable`1[System.Single], System.Nullable`1[System.Double], System.Nullable`1[System.Decimal], System.Nullable`1[System.Guid], System.Nullable`1[System.DateTime]";

            Assert.AreEqual(string.Format("Property 'value' mapping validation error, type '{0}' is not supported, supported types: [{1}]", typeof(object).AssemblyQualifiedName, supportedTypes), ex.Message);
        }

        [Test]
        public void CorrectMapping()
        {
            var propertyInfo = typeof(Model.Entity).GetMember("Id", BindingFlags.Public | BindingFlags.Instance).First();

            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();
            mappingMock.Setup(m => m.Name).Returns("id");
            mappingMock.Setup(m => m.Member).Returns(propertyInfo);

            var propertyMappingValidator = new PropertyMappingValidator(factoryMock.Object);
            propertyMappingValidator.Validate(mappingMock.Object, null);
        }
    }
}