using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DbMapper.Factories;
using DbMapper.MappingValidators;
using DbMapper.Utils;

namespace DbMapper
{
    public abstract class DefaultDbModule : IDbModule
    {
        private readonly MappingValidatorFactory _mappingValidatorFactory = new MappingValidatorFactory();
        protected IMappingValidatorFactory MappingValidatorFactory
        {
            get { return _mappingValidatorFactory; }
        }

        private readonly MappingProvider _mappingProvider;

        public IMappingProvider MappingProvider
        {
            get { return _mappingProvider; }
        }

        protected DefaultDbModule()
        {
            _mappingProvider = new MappingProvider(_mappingValidatorFactory);
        }

        protected internal virtual void RegisterValidators()
        {
            RegisterMappingValidator(f => new TableMappingValidator(f));
            RegisterMappingValidator(f => new TablePropertyMappingValidator(f));
            RegisterMappingValidator(f => new VersionPropertyMappingValidator(f));
            RegisterMappingValidator(f => new TableSubClassMappingValidator(f));

            RegisterMappingValidator(f => new ViewMappingValidator(f));
            RegisterMappingValidator(f => new ViewSubClassMappingValidator(f));

            RegisterMappingValidator(f => new DiscriminatorMappingValidator(f));
            RegisterMappingValidator(f => new SubClassJoinValidator(f));
            RegisterMappingValidator(f => new SubClassJoinColumnValidator(f));
            RegisterMappingValidator(f => new PropertyMappingValidator(f));


            RegisterMappingValidator(f => new ExtendTableMappingValidator(f));
            RegisterMappingValidator(f => new ExtendViewMappingValidator(f));

            RegisterMappingValidator(f => new ExtendTableMappingStatefulValidator(f));
            RegisterMappingValidator(f => new ExtendViewMappingStatefulValidator(f));
        }

        public Type QueryBuilderType { get; protected set; }

        public abstract IQueryBuilder CreateQueryBuilder();

        public void Configure(XElement xModule)
        {
            XElement xMappingBuilders;
            if (!xModule.TryGetElement("mapping-builders", out xMappingBuilders))
                throw new Exceptions.ConfigurationException("Cannot find mapping-builders");

            IList<XElement> xMappingBuildersList;
            if (!xMappingBuilders.TryGetElements("mapping-builder", out xMappingBuildersList))
                throw new Exceptions.ConfigurationException("Cannot find mapping-builders");

            foreach (var xMappingBuilder in xMappingBuildersList)
            {
                XAttribute xMappingBuilderClass;
                if (!xMappingBuilder.TryGetAttribute("class", out xMappingBuilderClass))
                    throw new Exceptions.ConfigurationException("Cannot find mapping-builder class");

                Type mappingBuilderType;
                try
                {
                    mappingBuilderType = xMappingBuilderClass.GetValueAsType();
                }
                catch (Exception ex)
                {
                    throw new Exceptions.ConfigurationException(string.Format("Cannot load mapping-builder '{0}'", xMappingBuilderClass.Value), ex);
                }

                if (!typeof(IMappingBuilder).IsAssignableFrom(mappingBuilderType))
                    throw new Exceptions.ConfigurationException(string.Format("Cannot load mapping-builder '{0}', mapping-builder must be inherited from '{1}'", xMappingBuilderClass.Value,
                        typeof(IMappingBuilder).AssemblyQualifiedName));

                var mappingBuilder = (IMappingBuilder)Activator.CreateInstance(mappingBuilderType);

                try
                {
                    mappingBuilder.Configure(xMappingBuilder);
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot initialize db-maper", ex);
                }

                _mappingProvider.RegisterMappingBuilder(mappingBuilder);
            }

            RegisterValidators();
        }

        protected void RegisterMappingValidator<T>(MappingValidatorCreator<T> creator) where T : IMappingValidator
        {
            _mappingValidatorFactory.RegisterValidator(creator);
        }
    }
}