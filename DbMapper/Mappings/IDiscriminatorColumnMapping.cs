using System;

namespace DbMapper.Mappings
{
    public interface IDiscriminatorColumnMapping
    {
        string Column { get; }

        Type Type { get; }
    }
}