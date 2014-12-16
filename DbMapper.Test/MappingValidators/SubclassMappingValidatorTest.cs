using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
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

    public class WrongTableSubEntity
    {
        public string Name { get; set; }
    }

    public abstract class AbstractSubEntity : Entity
    {
        public string Name { get; set; }
    }

    [TestFixture]
    public class SubClassMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => subClassMappingValidator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Subclass mapping validation error, mapping '{0}' is not a subclass mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullParent()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, parent class is null", typeof(SubEntity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void WrongParentType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var parentMappingMock = new Mock<IMappingClassReference>();
            parentMappingMock.Setup(m => m.Type).Returns(typeof(Entity));

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(WrongTableSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(parentMappingMock.Object);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, class '{0}' is not inherited from '{1}'",
                typeof(WrongTableSubEntity).AssemblyQualifiedName, typeof(Entity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NonAbstractClassNoDiscriminatorAndJoin()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));


            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, subclass has to have discriminator or join at least",
                typeof(SubEntity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NonAbstractClassOnlyJoin()
        {
            var factoryStub = new MappingValidatorFactoryStub();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var joinMapping = new Mock<ISubClassJoin>();

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Join).Returns(joinMapping.Object);

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryStub);
            subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object);
        }
    }
}