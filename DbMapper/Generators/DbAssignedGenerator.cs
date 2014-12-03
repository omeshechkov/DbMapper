using System;

namespace DbMapper.Generators
{
    public sealed class DbAssignedGenerator : IGenerator
    {
        public static readonly DbAssignedGenerator Instance = new DbAssignedGenerator();

        internal DbAssignedGenerator() { }
        
        public object GetNextValue(Connection connection)
        {
            throw new NotSupportedException("This generator type cannot get next value");
        }
    }
}
