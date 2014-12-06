namespace DbMapper.Mappings
{
    public interface ITablePropertyMapping : IPropertyMapping
    {
        IGenerator Generator { get; }

        bool Insert { get; }
        
        bool Update { get; }
    }
}