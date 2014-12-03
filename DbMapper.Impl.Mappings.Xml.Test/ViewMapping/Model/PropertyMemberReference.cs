namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model
{
    public class PropertyFieldReference
    {
        public static string StaticField;

        private string _field;

        public string Field;
    }

    public class PropertyPropertyReference
    {
        private string _noSetterProperty;
        public static string StaticProperty { get; set; }

        public string PublicProperty { get; set; }
        
        private string PrivateProperty { get; set; }

        public string NoSetterProperty
        {
            get { return _noSetterProperty; }
        }

        public string NoGetterProperty
        {
            set { _noSetterProperty = value; }
        }
    }
}