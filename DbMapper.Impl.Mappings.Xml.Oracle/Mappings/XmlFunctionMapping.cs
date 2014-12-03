using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Oracle.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlFunctionMapping : IFunctionMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-function-mapping";

        public XmlFunctionMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build function mapping", new ArgumentNullException("xMapping"));

            XElement xFunction;
            if (!xMapping.TryGetElement(XNamespace + "function", out xFunction))
                throw new DocumentParseException("Cannot find function at function mapping");

            XAttribute xSchema;
            if (xFunction.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xFunction.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at function mapping");

            Name = xName.Value;

            XAttribute xDelegate;
            if (!xFunction.TryGetAttribute("delegate", out xDelegate))
                throw new DocumentParseException("Cannot find delegate at function mapping");

            try
            {
                Type = xDelegate.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at function mapping", xDelegate.Value), ex);
            }

            if (!typeof(Delegate).IsAssignableFrom(Type))
                throw new DocumentParseException("'{0}' should be a delegate", Type.FullName);

            Delegate = Type.GetMethod("Invoke");

            Parameters = new List<IParameterMapping>();
            foreach (var xParameter in xFunction.Elements(XNamespace + "parameter"))
            {
                Parameters.Add(new XmlParameterMapping(Delegate, xParameter));
            }

            XElement xReturnValue;
            if (!xFunction.TryGetElement(XNamespace + "return-value", out xReturnValue))
                throw new DocumentParseException("Cannot find return-value at function mapping");

            ReturnValue = new XmlFunctionReturnValueMapping(xReturnValue);
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IParameterMapping> Parameters { get; private set; }

        public MethodInfo Delegate { get; private set; }

        public IFunctionReturnValueMapping ReturnValue { get; set; }
    }
}