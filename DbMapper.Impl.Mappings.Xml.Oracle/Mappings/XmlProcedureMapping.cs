using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Oracle.Mappings;
using DbMapper.Utils;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlProcedureMapping : IProcedureMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-procedure-mapping";

        public XmlProcedureMapping(XElement xMapping)
        {
            if (xMapping == null)
                throw new DocumentParseException("Cannot build procedure mapping", new ArgumentNullException("xMapping"));                

            XElement xProcedure;

            if (!xMapping.TryGetElement(XNamespace + "procedure", out xProcedure))
                throw new DocumentParseException("Cannot find procedure at procedure mapping");

            XAttribute xSchema;
            if (xProcedure.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            XAttribute xName;
            if (!xProcedure.TryGetAttribute("name", out xName))
                throw new DocumentParseException("Cannot find name at procedure mapping");

            Name = xName.Value;

            XAttribute xDelegate;
            if (!xProcedure.TryGetAttribute("delegate", out xDelegate))
                throw new DocumentParseException("Cannot find delegate at procedure mapping");

            try
            {
                Type = xDelegate.GetValueAsType();
            }
            catch (Exception ex)
            {
                throw new DocumentParseException(string.Format("Cannot recognize '{0}' class at procedure mapping", xDelegate.Value), ex);
            }


            Delegate = Type.GetMethod("Invoke");

            Parameters = new List<IParameterMapping>();
            foreach (var xParameter in xProcedure.Elements(XNamespace + "parameter"))
            {
                Parameters.Add(new XmlParameterMapping(Delegate, xParameter));
            }
        }

        public string Name { get; private set; }

        public string Schema { get; private set; }

        public Type Type { get; private set; }

        public IList<IParameterMapping> Parameters { get; private set; }

        public MethodInfo Delegate { get; private set; }
    }
}