using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IObjectMapping : IDbMapping, IHasProperties { }

    public interface IObjectPropertyMapping : IPropertyMapping { }
}