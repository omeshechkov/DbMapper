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
    public class SubclassMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(null, null));
            Assert.AreEqual("Subclass mapping validation error, mapping is null", ex.Message);
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
        public void NullType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<ISubClassMapping>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual("Subclass mapping validation error, type is null", ex.Message);
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
        public void WrongType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var parentMappingMock = new Mock<IMappingClassReference>();
            parentMappingMock.Setup(m => m.Type).Returns(typeof(Entity));

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(WrongTableSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(parentMappingMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, class '{0}' is not inherited from '{1}'",
                typeof(WrongTableSubEntity).AssemblyQualifiedName, typeof(Entity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NoContext()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, context is null",
                typeof(AbstractSubEntity).AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void WrongContextType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var contextMock = new Mock<IMappingClassReference>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, contextMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, context of type '{1}' doesn't contain discriminator column",
                typeof(AbstractSubEntity).AssemblyQualifiedName, contextMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void AbstractClassWithDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);
            mappingMock.Setup(m => m.DiscriminatorValue).Returns(123);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, abstract class cannot have discriminator-value",
                typeof(AbstractSubEntity).AssemblyQualifiedName), ex.Message);
        }
        
        [Test]
        public void AbstractClassWithoutDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(AbstractSubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object);
        }

        [Test]
        public void NonAbstractClassWithoutDiscriminatorValue()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, non abstact class with discriminator column should have discriminator-value",
                typeof(SubEntity).AssemblyQualifiedName), ex.Message);
        } 
        
        [Test]
        public void NonAbstractClassWrongDiscriminatorValueType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();
            discriminatorMappingMock.Setup(m => m.Type).Returns(typeof (long));

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);
            mappingMock.Setup(m => m.DiscriminatorValue).Returns("str");

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, discriminator value type is not match discriminator column type, expected: '{1}', actual: '{2}'",
                typeof(SubEntity).AssemblyQualifiedName, typeof(long).AssemblyQualifiedName, typeof(string).AssemblyQualifiedName), ex.Message);
        }  
        
        [Test]
        public void NonAbstractClassCorrectDiscriminatorValueType()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var discriminatorMappingMock = new Mock<IDiscriminatorMapping>();
            discriminatorMappingMock.Setup(m => m.Type).Returns(typeof (string));

            var mappingClassMock = new Mock<IMappingClassReference>();
            mappingClassMock.Setup(m => m.Type).Returns(typeof(Entity));

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();
            discriminatorContainerMock.Setup(m => m.Discriminator).Returns(discriminatorMappingMock.Object);

            var mappingMock = new Mock<ISubClassMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(SubEntity));
            mappingMock.Setup(m => m.Parent).Returns(mappingClassMock.Object);
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);
            mappingMock.Setup(m => m.DiscriminatorValue).Returns("str");

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object);
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
            mappingMock.Setup(m => m.Properties).Returns(new IPropertyMapping[0]);

            var discriminatorContainerMock = new Mock<IHasDiscriminator>();

            var subClassMappingValidator = new SubClassMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => subClassMappingValidator.Validate(mappingMock.Object, discriminatorContainerMock.Object));
            Assert.AreEqual(string.Format("Subclass mapping '{0}' validation error, subclass without discriminator has to have join",
                typeof(SubEntity).AssemblyQualifiedName), ex.Message);
        }
    }
}