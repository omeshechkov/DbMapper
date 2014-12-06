using System;
using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IObjectTableMapping : IDbMapping
    {
        Type ObjectType { get; set; }
    }
}