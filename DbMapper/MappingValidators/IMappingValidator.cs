namespace DbMapper.MappingValidators
{
    public interface IMappingValidator
    {
        void Validate(object mapping, object context);
    }

    public interface IStatefulMappingValidator : IMappingValidator
    {
        void BeginValidate();
        
        void EndValidate();
    }
}