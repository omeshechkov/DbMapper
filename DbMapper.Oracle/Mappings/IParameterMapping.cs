using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DbMapper.Oracle.Mappings
{
    public interface IHasParameters
    {
        IList<IParameterMapping> Parameters { get; }
    }

    public interface IParameterMapping
    {
        ParameterInfo Parameter { get; }

        string DbParameterName { get; }

        DbType DbType { get; set; }

        int Length { get; }
    }
}