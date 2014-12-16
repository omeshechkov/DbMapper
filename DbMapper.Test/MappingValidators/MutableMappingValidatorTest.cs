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
    public class MutableMappingValidatorTest
    {
        public class Entity
        {
            public long Id { get; set; }

            public long Value { get; set; }
        }

        public class SubEntity : Entity
        {
            public string Name { get; set; }
        }

        public abstract class AbstractSubEntity : Entity
        {
            public string Name { get; set; }
        }

        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var validator = new MutableMappingValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var validator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Mutable mapping validation error, mapping '{0}' is not a mutable mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NoContext()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(Entity));

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Mutable mapping '{0}' validation error, context is null", typeof(Entity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void WrongContextType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var contextMock = new Mock<IMappingClassReference>();

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(Entity));

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, contextMock.Object));
            Assert.AreEqual(string.Format("Mutable mapping '{0}' validation error, context of type '{1}' doesn't contain discriminator column",
                typeof(Entity).AssemblyQualifiedName, contextMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void AbstractClassWithDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));
            mappingMock.Setup(m => m.DiscriminatorValue).Returns(123);

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Mutable mapping '{0}' validation error, abstract class cannot have discriminator-value",
                typeof(AbstractSubEntity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void AbstractClassWithoutDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object);
        }

        [Test]
        public void NonAbstractClassWithoutDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Mutable mapping '{0}' validation error, non abstact class with discriminator column has to have discriminator-value",
                typeof(SubEntity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NonAbstractClassWrongDiscriminatorValueType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();
            discriminatorMappingMock.Setup(m => m.Type).Returns(typeof(long));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.DiscriminatorValue).Returns("str");

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Mutable mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                typeof(SubEntity).AssemblyQualifiedName, typeof(long).AssemblyQualifiedName, typeof(string).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NonAbstractClassCorrectDiscriminatorValueType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();
            discriminatorMappingMock.Setup(m => m.Type).Returns(typeof(string));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<IMutableMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.DiscriminatorValue).Returns("str");

            var subClassMappingValidator = new MutableMappingValidator(factoryMock.Object);
            subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object);
        }
    }
}