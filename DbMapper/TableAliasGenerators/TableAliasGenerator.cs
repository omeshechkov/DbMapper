using System.Collections.Generic;

namespace DbMapper.TableAliasGenerators
{
    public class TableAliasGenerator : ITableAliasGenerator
    {
        private readonly IDictionary<string, int> _usedNames = new Dictionary<string, int>();

        public string Generate(string table)
        {
            int c;
            _usedNames.TryGetValue(table, out c);
            c++;

            _usedNames[table] = c;

            return table + c;
        }
    }
}