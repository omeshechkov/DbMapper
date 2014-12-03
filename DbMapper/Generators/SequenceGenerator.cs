using System;

namespace DbMapper.Generators
{
    public sealed class SequenceGenerator : IGenerator
    {
        public string Name { get; private set; }

        public SequenceGenerator(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
        }

        public object GetNextValue(Connection connection)
        {
            throw new NotImplementedException();
        }
    }
}
