using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IObjectMapping : IMapping, IHasProperties { }

    public interface IObjectPropertyMapping : IPropertyMapping { }
}