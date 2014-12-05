namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model
{
    public class Shape
    {
        private long _id;
        public long _version;

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private long Version
        {
            get { return _version; }
            set { _version = value; }
        }
    }
    public class TwoDimensionalShape : Shape
    {
        public long X { get; set; }
        
        public long Y { get; set; }
    }

    public class ThreeDimensionalShape : Shape
    {
        public long X { get; set; }

        public long Y { get; set; }
    }

    public class Rectangle : TwoDimensionalShape
    {
        public long Width { get; set; }
        
        public long Height { get; set; }
    }

    public class Circle : TwoDimensionalShape
    {
        public long Width { get; set; }

        public long Height { get; set; }
    }
}
