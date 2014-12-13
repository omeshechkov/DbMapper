using System;
using System.Collections.Generic;
using DbMapper.Factories;
using DbMapper.Mappings;
using DbMapper.MappingValidators.Exceptions;

namespace DbMapper.MappingValidators
{
    [CanValidate(typeof(IViewMapping))]
    [CanValidate(typeof(IExtendViewMapping))]
    public sealed class ExtendViewMappingStatefulValidator : MappingValidator, IStatefulMappingValidator
    {
        private IList<Type> _registeredTypes;
        private IList<Type> _extendTypes;

        public ExtendViewMappingStatefulValidator(IMappingValidatorFactory factory)
            : base(factory)
        {
        }

        public override void Validate(object mapping, object context)
        {
            if (mapping == null)
                throw new ValidationException("Extend view validation error, mapping is null");

            var viewMapping = mapping as IViewMapping;
            var extendViewMapping = mapping as IExtendViewMapping;

            if (viewMapping == null && extendViewMapping == null)
                throw new ValidationException("Extend view or view mapping validation error, mapping '{0}' is not a view or extend-view mapping", mapping.GetType().AssemblyQualifiedName);

            if (viewMapping != null)
            {
                if (_registeredTypes.Contains(viewMapping.Type))
                    throw new ValidationException("Cannot register mapping of view-type, duplicate mapping for '{0}' type", viewMapping.Type.AssemblyQualifiedName);

                _registeredTypes.Add(viewMapping.Type);
                RegisterMapping(viewMapping);
            }

            if (extendViewMapping != null)
            {
                _extendTypes.Add(extendViewMapping.Type);
                RegisterMapping(extendViewMapping);
            } 
        }

        public void BeginValidate()
        {
            _registeredTypes = new List<Type>();
            _extendTypes = new List<Type>();
        }

        public void EndValidate()
        {
            foreach (var extendType in _extendTypes)
            {
                //Extend view type is not assigned from view, view subclass or extend-view subclass type
                if (!_registeredTypes.Contains(extendType))
                    throw new ValidationException("Extend view mapping validation error, there is no view/subclass mapping for '{0}'", extendType.AssemblyQualifiedName);
            }
        }

        private void RegisterMapping(IMutableMapping mapping)
        {
            if (_registeredTypes.Contains(mapping.Type))
                throw new ValidationException("Cannot register view/extend-view subclass mapping, duplicate mapping for '{0}' type", mapping.Type.AssemblyQualifiedName);

            foreach (var subClassMapping in mapping.SubClasses)
            {
                RegisterMapping(subClassMapping);
            }
        }
    }
}