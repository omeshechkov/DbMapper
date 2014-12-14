using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
{
    [TestFixture]
    public class SubclassJoinColumnValidatorTest 
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(null, null));
            Assert.AreEqual("Subclass join column mapping validation error, mapping is null", ex.Message);
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnMock = new Mock<IPropertyMapping>();

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(joinColumnMock.Object, null));
            Assert.AreEqual(string.Format("Subclass join column mapping validation error, mapping '{0}' is not a subclass column join mapping", joinColumnMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnMock = new Mock<ISubClassJoinColumn>();

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(joinColumnMock.Object, null));
            Assert.AreEqual("Subclass join column mapping validation error, name is null or empty", ex.Message);
        }

        [Test]
        public void EmptyName()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnMock = new Mock<ISubClassJoinColumn>();
            joinColumnMock.Setup(j => j.Name).Returns(string.Empty);

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(joinColumnMock.Object, null));
            Assert.AreEqual("Subclass join column mapping validation error, name is null or empty", ex.Message);
        }
 
        [Test]
        public void NullJoinColumn()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnMock = new Mock<ISubClassJoinColumn>();
            joinColumnMock.Setup(j => j.Name).Returns("src_column");

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(joinColumnMock.Object, null));
            Assert.AreEqual("Subclass join column mapping validation error, join column is null or empty", ex.Message);
        }

        [Test]
        public void EmptyJoinColumn()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinColumnMock = new Mock<ISubClassJoinColumn>();
            joinColumnMock.Setup(j => j.Name).Returns("src_column");
            joinColumnMock.Setup(j => j.JoinTable).Returns(string.Empty);

            var joinColumnValidator = new SubClassJoinColumnValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinColumnValidator.Validate(joinColumnMock.Object, null));
            Assert.AreEqual("Subclass join column mapping validation error, join column is null or empty", ex.Message);
        }      
    }
}