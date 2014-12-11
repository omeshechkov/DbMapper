namespace DbMapper.MappingValidators
{
    public interface IMappingValidator
    {
        void Validate(object mapping);
    }

    public interface IStatefulMappingValidator : IMappingValidator
    {
        void BeginValidate();
        
        void EndValidate();
    }
}