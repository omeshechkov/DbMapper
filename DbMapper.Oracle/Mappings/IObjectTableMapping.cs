using System;
using DbMapper.Mappings;

namespace DbMapper.Oracle.Mappings
{
    public interface IObjectTableMapping : IMapping
    {
        Type ObjectType { get; set; }
    }
}