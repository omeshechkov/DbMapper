using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
{
    [TestFixture]
    public class SubclassJoinValidatorTest 
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(null, null));
            Assert.AreEqual("Subclass join validation error, mapping is null", ex.Message);
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var joinMock = new Mock<IPropertyMapping>();

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(joinMock.Object, null));
            Assert.AreEqual(string.Format("Subclass join validation error, mapping '{0}' is not a subclass join mapping", joinMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullTable()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var joinMock = new Mock<ISubClassJoin>();

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(joinMock.Object, null));
            Assert.AreEqual("Subclass join validation error, table name is null or empty", ex.Message);
        }

        [Test]
        public void EmptyTable()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var joinMock = new Mock<ISubClassJoin>();
            joinMock.Setup(j => j.Table).Returns(string.Empty);

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(joinMock.Object, null));
            Assert.AreEqual("Subclass join validation error, table name is null or empty", ex.Message);
        }
 
        [Test]
        public void NullColumns()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var joinMock = new Mock<ISubClassJoin>();
            joinMock.Setup(j => j.Table).Returns("table");

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(joinMock.Object, null));
            Assert.AreEqual("Subclass join validation error, columns is null", ex.Message);
        }

        [Test]
        public void EmptyColumns()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var joinMock = new Mock<ISubClassJoin>();
            joinMock.Setup(j => j.Table).Returns("table");
            joinMock.Setup(j => j.ColumnJoins).Returns(new ISubClassJoinColumn[0]);

            var joinValidator = new SubClassJoinValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => joinValidator.Validate(joinMock.Object, null));
            Assert.AreEqual("Subclass join validation error, no columns", ex.Message);
        }      
    }
}