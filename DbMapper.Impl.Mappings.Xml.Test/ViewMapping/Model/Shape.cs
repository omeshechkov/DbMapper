namespace DbMapper.Impl.Mappings.Xml.Test.ViewMapping.Model
{
    public class Shape
    {
        public long Id { get; set; }

        public long Version { get; set; }
    }

    public class TwoDimensionalShape : Shape
    {
        public long X { get; set; }
        
        public long Y { get; set; }
    }

    public class Rectangle : TwoDimensionalShape
    {
        public long Width { get; set; }
        
        public long Height { get; set; }
    }
}
