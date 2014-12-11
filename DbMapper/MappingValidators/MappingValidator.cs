using DbMapper.Factories;

namespace DbMapper.MappingValidators
{
    public abstract class MappingValidator : IMappingValidator
    {
        internal IMappingValidatorFactory Factory { get; private set; }

        public abstract void Validate(object mapping);

        protected MappingValidator(IMappingValidatorFactory factory)
        {
            Factory = factory;
        }
    }
}