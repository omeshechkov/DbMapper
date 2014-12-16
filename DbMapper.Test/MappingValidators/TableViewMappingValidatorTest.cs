using System;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators;
using DbMapper.MappingValidators.Exceptions;
using Moq;
using NUnit.Framework;

namespace DbMapper.Test.MappingValidators
{
    public class TableViewEntity
    {
        public long Id { get; set; }

        public long Value { get; set; }
    }

    [TestFixture]
    public class TableViewMappingValidatorTest
    {
        [Test]
        public void NullMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();

            var validator = new TableViewMappingValidator(factoryMock.Object);
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, null));
        }

        [Test]
        public void WrongMapping()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<IPropertyMapping>();

            var validator = new TableViewMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Table/view mapping validation error, mapping '{0}' is not a table or view mapping", mappingMock.Object.GetType().AssemblyQualifiedName), ex.Message);
        }

        [Test]
        public void NullProperties()
        {
            var factoryMock = new Mock<IMappingValidatorFactory>();
            var mappingMock = new Mock<ITableViewMapping>();
            mappingMock.Setup(m => m.Type).Returns(typeof(Entity));

            var validator = new TableViewMappingValidator(factoryMock.Object);
            var ex = Assert.Throws<ValidationException>(() => validator.Validate(mappingMock.Object, null));
            Assert.AreEqual(string.Format("Table/view mapping '{0}' validation error, no properties", typeof(Entity).AssemblyQualifiedName), ex.Message);
        }
    }
}