using System;
using System.Collections.Generic;
using System.Reflection;
using DbMapper.Impl.Mappings.Xml.Exceptions;

namespace DbMapper.Impl.Mappings.Xml.Utils
{
    public class TypeUtils
    {
        private static readonly IDictionary<string, Type> Shorthands = new Dictionary<string, Type>
        {
            { "bool", typeof(bool) },

            { "string", typeof(string) },
            { "type", typeof(Type) },
            
            { "date-time", typeof(DateTime) },

            { "byte", typeof(byte) },
            { "short", typeof(short) },
            { "int", typeof(int) },
            { "long", typeof(long) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },

            { "guid", typeof(Guid) }
        };

        public static bool TryParseType(string typeString, bool onlyShorthands, out Type type)
        {
            var isNullable = typeString[typeString.Length - 1] == '?';

            if (isNullable)
                typeString = typeString.Substring(0, typeString.Length - 1);

            if (!Shorthands.TryGetValue(typeString, out type))
            {
                if (onlyShorthands)
                    return false;

                type = Type.GetType(typeString, true);
            }

            if (!isNullable) 
                return true;

            if (!type.IsValueType)
                throw new ParseTypeException("Cannot make type '{0}' nullable, type isn't a value type", typeString);

            type = typeof(Nullable<>).MakeGenericType(type);

            return true;
        }

        public static Type ParseType(string typeString, bool onlyShorthands)
        {
            Type type;
            if (!TryParseType(typeString, onlyShorthands, out type))
                throw new ParseTypeException("Unknown type '{0}'", typeString);                

            return type;
        }
        
        public static Type GetMemberType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).FieldType;

                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
            }

            throw new DocumentParseException("Unknown member type '{0}' for member '{1}", memberInfo.MemberType, memberInfo.Name);
        }

        public static object ParseAs(Type type, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                if (Nullable.GetUnderlyingType(type) == null)
                    throw new ParseValueException(str, type);

                return null;
            }

            if (type == typeof(bool))
                return bool.Parse(str);

            if (type == typeof(string))
                return str;

            if (type == typeof(Type))
                return Type.GetType(str, true);

            if (type == typeof(DateTime))
                return DateTime.Parse(str);

            if (type == typeof(byte))
                return byte.Parse(str);

            if (type == typeof(short))
                return short.Parse(str);

            if (type == typeof(int))
                return int.Parse(str);

            if (type == typeof(long))
                return long.Parse(str);

            if (type == typeof(float))
                return float.Parse(str);

            if (type == typeof(double))
                return double.Parse(str);

            if (type == typeof(decimal))
                return decimal.Parse(str);

            throw new ParseValueException("Unsupported type '{0}'", type);
        }
    }
}