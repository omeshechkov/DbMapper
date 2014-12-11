using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbMapper.MappingValidators;

namespace DbMapper.Factories
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CanValidateAttribute : Attribute
    {
        public Type Type { get; set; }

        public CanValidateAttribute(Type type)
        {
            Type = type;
        }
    }

    public delegate T MappingValidatorCreator<out T>(IMappingValidatorFactory factory) where T : IMappingValidator;

    internal class MappingValidatorFactory : IMappingValidatorFactory
    {
        private readonly IDictionary<Type, IList<MappingValidatorCreatorInfo>> _mappingValidators = new Dictionary<Type, IList<MappingValidatorCreatorInfo>>();

        public void RegisterValidator<T>(MappingValidatorCreator<T> creator) where T : IMappingValidator
        {
            var mappingValidatorType = typeof(T);
            var canValidateList = mappingValidatorType.GetCustomAttributes<CanValidateAttribute>().ToList();

            if (canValidateList.Count == 0)
                throw new Exception(string.Format("Cannot register mapping validator of type '{0}', there are no supported validation types, use '{1}' attribute to register them",
                    mappingValidatorType.AssemblyQualifiedName, typeof(CanValidateAttribute).AssemblyQualifiedName));

            foreach (var canValidateAttribute in canValidateList)
            {
                IList<MappingValidatorCreatorInfo> mappingValidators;
                if (!_mappingValidators.TryGetValue(canValidateAttribute.Type, out mappingValidators))
                {
                    mappingValidators = new List<MappingValidatorCreatorInfo>();
                    _mappingValidators.Add(canValidateAttribute.Type, mappingValidators);
                }

                var info = new MappingValidatorCreatorInfo
                {
                    MappingValidatorType = typeof(T),
                    Creator = creator
                };

                mappingValidators.Add(info);
            }
        }

        public IDictionary<Type, IList<IMappingValidator>> GetMappingValidators(Type type)
        {
            var result = new Dictionary<Type, IList<IMappingValidator>>();
            var instances = new Dictionary<Type, IMappingValidator>();

            foreach (var kvp in _mappingValidators.Where(kvp => kvp.Key.IsAssignableFrom(type)))
            {
                var validationObjectType = kvp.Key;
                var creatorsInfo = kvp.Value;

                var validators = new List<IMappingValidator>();

                foreach (var creatorInfo in creatorsInfo)
                {
                    IMappingValidator mappingValidator;
                    if (!instances.TryGetValue(creatorInfo.MappingValidatorType, out mappingValidator))
                    {
                        mappingValidator = ((MappingValidatorCreator<IMappingValidator>)creatorInfo.Creator)(this);
                        instances.Add(creatorInfo.MappingValidatorType, mappingValidator);
                    }

                    validators.Add(mappingValidator);
                }

                result[validationObjectType] = validators;
            }
            
            return result;
        }

        public IDictionary<Type, IList<IMappingValidator>> GetMappingValidators<T>()
        {
            return GetMappingValidators(typeof(T));
        }

        private class MappingValidatorCreatorInfo
        {
            public Type MappingValidatorType;

            public Delegate Creator;
        }
    }
}