using System.Reflection;

namespace DbMapper.Mappings
{
    public interface IPropertyMapping : IHasConverter
    {
        /// <summary>
        /// Database entity property (table or view column, db-type property etc.)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// .NET Object field or property
        /// </summary>
        MemberInfo Member { get; }
   }
}