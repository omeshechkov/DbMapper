using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DbMapper.Impl.Mappings.Xml.Exceptions;
using DbMapper.Impl.Mappings.Xml.Utils;
using DbMapper.Oracle.Mappings;

namespace DbMapper.Impl.Mappings.Xml.Oracle.Mappings
{
    sealed class XmlProcedureMapping : IProcedureMapping
    {
        private static readonly XNamespace XNamespace = "urn:dbm-oracle-procedure-mapping";

        public XmlProcedureMapping(XElement xMapping)
        {
            var xProcedure = xMapping.Element(XNamespace + "procedure");

            XAttribute xSchema;
            if (xProcedure.TryGetAttribute("schema", out xSchema))
            {
                Schema = xSchema.Value;
            }

            Name = xProcedure.Attribute("name").Value;

            string delegateFullPath;

            XAttribute xClass;
            if (xProcedure.TryGetAttribute("class", out xClass))
            {
                var @class = xClass.GetAsType();
                var delegateName = xProcedure.Attribute("delegate").Value;

                delegateFullPath = string.Format("{0}.{1}", @class, delegateName);

                var member = @class.GetMember(delegateName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

                if (member == null)
                    throw new DocumentParseException("Cannot find static member '{0}' of '{1}' class", delegateName, @class.FullName);

                Type = TypeUtils.GetMemberType(member);
            }
            else
            {
                Type = xProcedure.Attribute("delegate").GetAsType();

                delegateFullPath = Type.FullName;
            }

            if (!typeof(Delegate).IsAssignableFrom(Type))
                throw new DocumentParseException("'{0}' should be a delegate", delegateFullPath);

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