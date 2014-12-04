using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Converters;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Factories;
using DbMapper.Impl.Mappings.Xml.Mappings;
using DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model;
using DbMapper.Mappings;
using NUnit.Framework;

namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping
{
    [TestFixture]
    class ViewMappingUnitTest
    {
        private Assembly _currentAssembly;

        [TestFixtureSetUp]
        public void Setup()
        {
            //Register builders and schemas
            new XmlMappingBuilder();

            _currentAssembly = Assembly.GetAssembly(typeof(ViewMappingUnitTest));
        }

        private XDocument GetMappingDocument(string resourceName)
        {
            resourceName = "DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Resources." + resourceName;
            var stream = _currentAssembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception(string.Format("Cannot find resource '{0}'", resourceName));

            return XDocument.Load(stream);
        }        

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoProperties()
        {
            var document = GetMappingDocument("View.NoProperties.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoViewName()
        {
            var document = GetMappingDocument("View.NoViewName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void NoViewClass()
        {
            var document = GetMappingDocument("View.NoViewName.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void NoSchema()
        {
            var document = GetMappingDocument("View.NoSchema.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.IsNull(mapping.Schema);
        }

        [Test]
        public void NoDiscriminatorValue()
        {
            var document = GetMappingDocument("View.CheckClassAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);

            Assert.IsNull(mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckSchemaName()
        {
            var document = GetMappingDocument("View.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual("test_dbm", mapping.Schema);
        }

        [Test]
        public void CheckTableName()
        {
            var document = GetMappingDocument("View.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual("shapes", mapping.Name);
        }

        [Test]
        public void CheckViewClass()
        {
            var document = GetMappingDocument("View.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.AreEqual(typeof(Shape), mapping.Type);
        }

        [Test]
        public void CheckMappingType()
        {
            var document = GetMappingDocument("View.CheckClassAttributes.xml");
            var mapping = MappingFactory.CreateMapping(document);

            Assert.IsInstanceOf<XmlViewMapping>(mapping);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void ClassDiscriminatorValueWithoutColumn()
        {
            var document = GetMappingDocument("View.DiscriminatorValueWithoutColumn.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        [ExpectedException(typeof(DocumentParseException))]
        public void WrongClassDiscriminatorValueType()
        {
            var document = GetMappingDocument("View.WrongDiscriminatorValueType.xml");
            MappingFactory.CreateMapping(document);
        }

        [Test]
        public void CheckClassDiscriminatorStringValue()
        {
            var document = GetMappingDocument("View.CheckDiscriminatorStringValue.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            Assert.AreEqual("Shape", mapping.DiscriminatorValue);
        }

        [Test]
        public void CheckClassDiscriminatorLongValue()
        {
            var document = GetMappingDocument("View.CheckDiscriminatorLongValue.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
        public void NoPropertyConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyOptionalAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
            Assert.IsNull(idProperty.Converter);
        }

        [Test]
        public void CheckPropertyPublicFieldReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPublicFieldReference.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
            var fieldInfo = typeof(PropertyFieldReference).GetMember("Field", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(fieldInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyPrivateFieldReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPrivateFieldReference.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("PublicProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyPrivatePropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyPrivatePropertyReference.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("PrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyNoSetterPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyNoSetterPropertyReference.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = mapping.Properties.First();
            var propertyInfo = typeof(PropertyPropertyReference).GetMember("NoSetterProperty", BindingFlags.Instance | BindingFlags.Public).First();
            Assert.AreEqual(propertyInfo, idProperty.Member);
        }

        [Test]
        public void CheckPropertyNoGetterPropertyReference()
        {
            var document = GetMappingDocument("Property.CheckPropertyNoGetterPropertyReference.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
            Assert.IsInstanceOf<YesNoConverter>(idProperty.Converter);
        }

        [Test]
        public void CheckPropertyConverter()
        {
            var document = GetMappingDocument("Property.CheckPropertyConverter.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var idProperty = (IViewPropertyMapping)mapping.Properties.First();
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var discriminator = mapping.Discriminator;
            Assert.AreEqual("type", discriminator.Column);
        }

        [Test]
        public void CheckDiscriminatorColumnType()
        {
            var document = GetMappingDocument("Discriminator.CheckDiscriminatorColumnAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var discriminator = mapping.Discriminator;
            Assert.AreEqual(typeof(string), discriminator.Type);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            Assert.AreEqual("Rectangle", subClassMapping.DiscriminatorValue);
        }

        [Test]
        public void CheckSubclassDiscriminatorLongValue()
        {
            var document = GetMappingDocument("Subclass.CheckDiscriminatorLongValue.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var schemaName = subClassMapping.Join.Schema;
            Assert.IsNull(schemaName);
        }

        [Test]
        public void CheckSubclassJoinTableName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var tableName = subClassMapping.Join.Table;
            Assert.AreEqual("rectangles", tableName);
        }

        [Test]
        public void CheckSubclassJoinSchemaName()
        {
            var document = GetMappingDocument("Subclass.CheckJoinAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.IsNull(columnJoin.JoinSchema);
        }

        [Test]
        public void CheckJoinNoColumnJoinTable()
        {
            var document = GetMappingDocument("Subclass.CheckJoinNoColumnJoinTable.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
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
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("rectangle_id", columnJoin.Name);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinSchema()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("test_dbm", columnJoin.JoinSchema);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinTable()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("shapes", columnJoin.JoinTable);
        }

        [Test]
        public void CheckSubclassJoinColumnJoinColumn()
        {
            var document = GetMappingDocument("Subclass.CheckJoinColumnAttributes.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var columnJoin = subClassMapping.Join.ColumnJoins.First();
            Assert.AreEqual("id", columnJoin.JoinColumn);
        }

        [Test]
        public void CheckSubclassSubclass()
        {
            var document = GetMappingDocument("Subclass.CheckSubclassSubclass.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var subSubClassMapping = subClassMapping.SubClasses.First();
            Assert.AreEqual(typeof(Rectangle), subSubClassMapping.Type);
        }

        [Test]
        public void CheckSubclassProperties()
        {
            var document = GetMappingDocument("Subclass.CheckSubclassProperties.xml");
            var mapping = (IViewMapping)MappingFactory.CreateMapping(document);
            var subClassMapping = mapping.SubClasses.First();
            var properties = subClassMapping.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.IsInstanceOf<XmlTablePropertyMapping>(properties.First());
        }
    }
}