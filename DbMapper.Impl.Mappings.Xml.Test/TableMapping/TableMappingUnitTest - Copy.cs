using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using DbMapper.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableMappingUnitTestOld
    {
        private Assembly _currentAssembly;

        [TestFixtureSetUp]
        public void Setup()
        {
            //Register builders and schemas
            new XmlMappingBuilder();

            _currentAssembly = Assembly.GetAssembly(typeof(TableMappingUnitTest));
        }

        private XDocument GetMappingDocument(string resourceName)
        {
            resourceName = "DbMapper.Impl.Mappings.Xml.Test.TableMapping.Resources." + resourceName;
            var stream = _currentAssembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception(string.Format("Cannot find resource '{0}'", resourceName));

            return XDocument.Load(stream);
        }        

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoSubclassName()
        {
            var document = GetMappingDocument("Subclass.NoSubclassName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckSubclassName()
        {
            var document = GetMappingDocument("Subclass.CheckSubclassName.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            Assert.AreEqual(typeof(Rectangle), subClassMapping.Type);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void ClassSubclassDiscriminatorValueWithoutColumn()
        {
            var document = GetMappingDocument("Subclass.DiscriminatorValueWithoutColumn.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void WrongSubclassDiscriminatorValueType()
        {
            var document = GetMappingDocument("Subclass.WrongDiscriminatorValueType.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckSubclassDiscriminatorStringValue()
        {
            var document = GetMappingDocument("Subclass.CheckDiscriminatorStringValue.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            Assert.AreEqual("Rectangle", subClassMapping.DiscriminatorValue);
        }

        [Test]
        public void CheckSubclassDiscriminatorLongValue()
        {
            var document = GetMappingDocument("Subclass.CheckDiscriminatorLongValue.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            Assert.AreEqual(321L, subClassMapping.DiscriminatorValue);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckSubclassEmptyJoin()
        {
            var document = GetMappingDocument("Subclass.CheckEmptyJoin.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckSubclassNoJoinTable()
        {
            var document = GetMappingDocument("Subclass.CheckNoJoinTable.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckSubclassNoJoinSchema()
        {
            var document = GetMappingDocument("Subclass.CheckNoJoinSchema.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var schemaName = subClassMapping.Join.Schema;
            Assert.IsNull(schemaName);
        }

        [Test]
        public void CheckSubclassJoinTableName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var tableName = subClassMapping.Join.Name;
            Assert.AreEqual("rectangles", tableName);
        }

        [Test]
        public void CheckSubclassJoinSchemaName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var schemaName = subClassMapping.Join.Schema;
            Assert.AreEqual("test_dbm", schemaName);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckSubclassJoinNoColumnName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinNoColumnName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckJoinNoColumnJoinSchema()
        {
            var document = GetMappingDocument("Subclass.CheckJoinNoColumnJoinSchema.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.IsNull(columnJoin.JoinSchema);
        }

        [Test]
        public void CheckJoinNoColumnJoinTable()
        {
            var document = GetMappingDocument("Subclass.CheckJoinNoColumnJoinTable.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.IsNull(columnJoin.JoinTable);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckJoinNoColumnJoinColumn()
        {
            var document = GetMappingDocument("Subclass.CheckJoinNoColumnJoinColumn.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckSubclassJoinColumnName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("rectangle_id", columnJoin.Name);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinSchema()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("test_dbm", columnJoin.JoinSchema);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinTable()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("shapes", columnJoin.JoinTable);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinColumn()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("id", columnJoin.JoinColumn);
        }

        [Test]
        public void CheckSubclassSubclass()
        {
            var document = GetMappingDocument("Subclass.CheckSubclassSubclass.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var subSubClassMapping = subClassMapping.SubClasses.First();
            Assert.AreEqual(typeof(Rectangle), subSubClassMapping.Type);
        }

        [Test]
        public void CheckSubclassProperties()
        {
            var document = GetMappingDocument("Subclass.CheckSubclassProperties.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var properties = subClassMapping.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.IsInstanceOf<XmlTablePropertyMapping>(properties.First());
        }
    }
}