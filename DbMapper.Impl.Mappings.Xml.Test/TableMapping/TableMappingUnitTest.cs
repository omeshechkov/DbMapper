using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Generators;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.TableMapping.Model;
using DbMapper.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.TableMapping
{
    [TestFixture]
    class TableMappingUnitTest
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
        public void NoProperties()
        {
            var document = GetMappingDocument("Table.NoProperties.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoTableName()
        {
            var document = GetMappingDocument("Table.NoTableName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoClassName()
        {
            var document = GetMappingDocument("Table.NoClassName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void NoSchema()
        {
            var document = GetMappingDocument("Table.NoSchema.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.IsNull(mapping.Schema);
        }

        [Test]
        public void NoDiscriminatorValue()
        {
            var document = GetMappingDocument("Table.CheckClassAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);

            Assert.IsNull(mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckSchemaName()
        {
            var document = GetMappingDocument("Table.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual("test_dbm", mapping.Schema);
        }

        [Test]
        public void CheckTableName()
        {
            var document = GetMappingDocument("Table.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual("shapes", mapping.Name);
        }

        [Test]
        public void CheckClassName()
        {
            var document = GetMappingDocument("Table.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual(typeof(Shape), mapping.Type);
        }

        [Test]
        public void CheckMappingType()
        {
            var document = GetMappingDocument("Table.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.IsInstanceOf<XmlTableMapping>(mapping);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void ClassDiscriminatorValueWithoutColumn()
        {
            var document = GetMappingDocument("Table.DiscriminatorValueWithoutColumn.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void WrongClassDiscriminatorValueType()
        {
            var document = GetMappingDocument("Table.WrongDiscriminatorValueType.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckClassDiscriminatorStringValue()
        {
            var document = GetMappingDocument("Table.CheckDiscriminatorStringValue.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            Assert.AreEqual("Shape", mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckClassDiscriminatorLongValue()
        {
            var document = GetMappingDocument("Table.CheckDiscriminatorLongValue.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            Assert.AreEqual(123L, mapping.DiscriminatorValue);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoPropertyName()
        {
            var document = GetMappingDocument("Property.NoPropertyName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoPropertyColumn()
        {
            var document = GetMappingDocument("Property.NoPropertyColumn.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void NoPropertyInsert()
        {
            var document = GetMappingDocument("Property.CheckPropertyOptionalAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsTrue(idProperty.Insert);
        }

        [Test]
        public void NoPropertyUpdate()
        {
            var document = GetMappingDocument("Property.CheckPropertyOptionalAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsTrue(idProperty.Update);
        }

        [Test]
        public void NoPropertyConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyOptionalAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsNull(idProperty.Converter);
        }

        [Test]
        public void NoPropertyGenerator()
        {
            var document = GetMappingDocument("Property.CheckPropertyOptionalAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsNull(idProperty.Generator);
        }

        [Test]
        public void CheckPropertyInsertTrue()
        {
            var document = GetMappingDocument("Property.CheckPropertyInsertTrue.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsTrue(idProperty.Insert);
        }

        [Test]
        public void CheckPropertyInsertFalse()
        {
            var document = GetMappingDocument("Property.CheckPropertyInsertFalse.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsFalse(idProperty.Insert);
        }

        [Test]
        public void CheckPropertyInsertTrue1()
        {
            var document = GetMappingDocument("Property.CheckPropertyInsertTrue1.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsTrue(idProperty.Insert);
        }

        [Test]
        public void CheckPropertyInsertFalse0()
        {
            var document = GetMappingDocument("Property.CheckPropertyInsertFalse0.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsFalse(idProperty.Insert);
        }

        [Test]
        public void CheckPropertyPublicFieldReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPublicFieldReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            var fieldInfo = typeof(PropertyFieldReference).GetMember("Field", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyPrivateFieldReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPrivateFieldReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            var fieldInfo = typeof(PropertyFieldReference).GetMember("_field", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyStaticFieldReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyStaticFieldReference.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckPropertyPublicPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPublicPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("PublicProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyPrivatePropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPrivatePropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("PrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyNoSetterPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyNoSetterPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("NoSetterProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyNoGetterPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyNoGetterPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("NoGetterProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyStaticPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyStaticPropertyReference.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckPropertyConverterShorthand()
        {
            var document = GetMappingDocument("Property.CheckPropertyConverterShorthand.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsInstanceOf<YesNoConverter>(idProperty.Converter);
        }

        [Test]
        public void CheckPropertyConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyConverter.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsInstanceOf<LowerYesNoConverter>(idProperty.Converter);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyPseudoConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyPseudoConverter.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyUnknownConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyWrongConverter.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyEmptyGenerator()
        {
            var document = GetMappingDocument("Property.CheckPropertyEmptyGenerator.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPropertyWrongGenerator()
        {
            var document = GetMappingDocument("Property.CheckPropertyWrongGenerator.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckPropertyDbAssignedGenerator()
        {
            var document = GetMappingDocument("Property.CheckPropertyDbAssignedGenerator.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsInstanceOf<DbAssignedGenerator>(idProperty.Generator);
        }

        [Test]
        public void CheckPropertySequenceGenerator()
        {
            var document = GetMappingDocument("Property.CheckPropertySequenceGenerator.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            Assert.IsInstanceOf<SequenceGenerator>(idProperty.Generator);
        }

        [Test]
        public void CheckPropertySequenceGeneratorName()
        {
            var document = GetMappingDocument("Property.CheckPropertySequenceGenerator.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = (ITablePropertyMapping)mapping.Properties.First();
            var sequenceGenerator = (SequenceGenerator)idProperty.Generator;
            Assert.AreEqual(sequenceGenerator.Name, "seq_shapes");
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoSequenceGeneratorName()
        {
            var document = GetMappingDocument("Property.NoSequenceGeneratorName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoDiscriminatorColumnName()
        {
            var document = GetMappingDocument("Discriminator.NoDiscriminatorColumnName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoDiscriminatorColumnType()
        {
            var document = GetMappingDocument("Discriminator.NoDiscriminatorColumnType.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void WrongDiscriminatorColumnType()
        {
            var document = GetMappingDocument("Discriminator.WrongDiscriminatorColumnType.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckDiscriminatorColumnName()
        {
            var document = GetMappingDocument("Discriminator.CheckDiscriminatorColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var discriminator = mapping.Discriminator;
            Assert.AreEqual("type", discriminator.Column);
        }

        [Test]
        public void CheckDiscriminatorColumnType()
        {
            var document = GetMappingDocument("Discriminator.CheckDiscriminatorColumnAttributes.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var discriminator = mapping.Discriminator;
            Assert.AreEqual(typeof(string), discriminator.Type);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoVersionPropertyName()
        {
            var document = GetMappingDocument("Version.NoVersionPropertyName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoVersionColumnName()
        {
            var document = GetMappingDocument("Version.NoVersionColumnName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckVersionColumnName()
        {
            var document = GetMappingDocument("Version.CheckVersionColumnName.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var versionProperty = mapping.VersionProperty;
            Assert.AreEqual("version", versionProperty.Name);
        }

        [Test]
        public void CheckVersionNullConverter()
        {
            var document = GetMappingDocument("Version.CheckVersionNullConverter.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var versionProperty = mapping.VersionProperty;
            Assert.IsNull(versionProperty.Converter);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckVersionPseudoConverter()
        {
            var document = GetMappingDocument("Version.CheckVersionPseudoConverter.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckVersionConverter()
        {
            var document = GetMappingDocument("Version.CheckVersionConverter.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var versionProperty = mapping.VersionProperty;
            Assert.IsInstanceOf<LowerYesNoConverter>(versionProperty.Converter);
        }    
        
        [Test]
        public void CheckVersionConverterShorthand()
        {
            var document = GetMappingDocument("Version.CheckVersionConverterShorthand.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var versionProperty = mapping.VersionProperty;
            Assert.IsInstanceOf<YesNoConverter>(versionProperty.Converter);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckVersionWrongConverter()
        {
            var document = GetMappingDocument("Version.CheckVersionWrongConverter.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckVersionNoGetterPropertyReference()
        {
            var document = GetMappingDocument("Version.CheckVersionNoGetterPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var propertyInfo = typeof(VersionPropertyReference).GetMember("NoGetterProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckVersionNoSetterPropertyReference()
        {
            var document = GetMappingDocument("Version.CheckVersionNoSetterPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var propertyInfo = typeof(VersionPropertyReference).GetMember("NoSetterProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckVersionPrivateFieldReference()
        {
            var document = GetMappingDocument("Version.CheckVersionPrivateFieldReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var fieldInfo = typeof(VersionFieldReference).GetMember("_field", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        public void CheckVersionPrivatePropertyReference()
        {
            var document = GetMappingDocument("Version.CheckVersionPrivatePropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var fieldInfo = typeof(VersionPropertyReference).GetMember("PrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        public void CheckVersionPublicFieldReference()
        {
            var document = GetMappingDocument("Version.CheckVersionPublicFieldReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var fieldInfo = typeof(VersionFieldReference).GetMember("Field", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        public void CheckVersionPublicPropertyReference()
        {
            var document = GetMappingDocument("Version.CheckVersionPublicPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.VersionProperty;
            var fieldInfo = typeof(VersionPropertyReference).GetMember("PublicProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckVersionStaticFieldReference()
        {
            var document = GetMappingDocument("Version.CheckVersionStaticFieldReference.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckVersionStaticPropertyReference()
        {
            var document = GetMappingDocument("Version.CheckVersionStaticPropertyReference.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoPrimaryKeyName()
        {
            var document = GetMappingDocument("PrimaryKey.NoPrimaryKeyName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void WrongPrimaryKeyName()
        {
            var document = GetMappingDocument("PrimaryKey.WrongPrimaryKeyName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void CheckPrimaryKeyPropertyReference()
        {
            var document = GetMappingDocument("PrimaryKey.CheckPrimaryKeyPropertyReference.xml");
            var mapping = (ITableMapping)MappingFactory.CreateMapping(document);
            var primaryKeyProperties = mapping.PrimaryKeyProperties.ToArray();
            var idPropertyInfo = typeof(PrimaryKeyMemberReference).GetMember("Id", BindingFlags.Instance | BindingFlags.Public).First();
            var timeAndZoneFieldInfo = typeof(PrimaryKeyMemberReference).GetMember("TimeAndZone", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.Contains(idPropertyInfo, primaryKeyProperties);
            Assert.Contains(timeAndZoneFieldInfo, primaryKeyProperties);
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