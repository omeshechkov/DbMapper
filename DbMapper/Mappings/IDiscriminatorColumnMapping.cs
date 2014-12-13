using System;

namespace DbMapper.Mappings
{
    public interface IDiscriminatorMapping
    {
        string Column { get; }

        Type Type { get; }
    }
}