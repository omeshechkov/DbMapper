using System.Reflection;
using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IProcedureMapping : IDbMapping, IHasParameters
    {
        MethodInfo Delegate { get; }
    }
}