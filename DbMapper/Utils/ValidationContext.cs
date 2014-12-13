using System;
using System.Collections.Generic;
using System.Linq;
using DbMapper.Factories;
using DbMapper.MappingValidators;

namespace DbMapper.Utils
{
    public class ValidationContext<T> : IDisposable
    {
        private bool _isDisposed;

        private readonly IDictionary<Type, IList<IMappingValidator>> _validators;
        private readonly IList<IStatefulMappingValidator> _statefulValidators = new List<IStatefulMappingValidator>();

        public ValidationContext(IMappingValidatorFactory factory)
        {
            _validators = factory.GetMappingValidators<T>();

            foreach (var validator in _validators.Values.SelectMany(v => v))
            {
                var statefulMappingValidator = validator as IStatefulMappingValidator;
                if (statefulMappingValidator == null) 
                    continue;

                _statefulValidators.Add(statefulMappingValidator);
                statefulMappingValidator.BeginValidate();
            }
        }

        public void Validate(object obj, object context = null)
        {
            foreach (var mappingValidator in _validators.Where(kvp => kvp.Key.IsInstanceOfType(obj)).SelectMany(kvp => kvp.Value))
            {
                mappingValidator.Validate(obj, context);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            foreach (var statefulMappingValidator in _statefulValidators)
            {
                statefulMappingValidator.EndValidate();
            }

            _isDisposed = true;
        }
    }
}