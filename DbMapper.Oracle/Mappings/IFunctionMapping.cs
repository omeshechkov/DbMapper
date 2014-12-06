using System.Data;
using System.Reflection;
using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IFunctionMapping : IDbMapping, IHasParameters
    {
        MethodInfo Delegate { get; }

        IFunctionReturnValueMapping ReturnValue { get; set; }
    }

    public interface IFunctionReturnValueMapping
    {
        IConverter Converter { get; }

        DbType DbType { get; set; }

        int Length { get; }
    }
}