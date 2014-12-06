using DbMapper.Mappings;

namespace DbMapper
{
    public interface IMappingValidator<T> where T : IMappingClassReference
    {
        void Validate(T mapping);
    }
}
