using System.Reflection;
using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IProcedureMapping : IMapping, IHasParameters
    {
        MethodInfo Delegate { get; }
    }
}