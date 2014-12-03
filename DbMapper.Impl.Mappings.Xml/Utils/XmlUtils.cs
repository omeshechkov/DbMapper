using System;
using System.Linq;
using System.Xml.Linq;

namespace DbMapper.Impl.Mappings.Xml.Utils
{
    public static class XmlUtils
    {
        private static string GetAsString(this XObject xObject)
        {
            var xElement = xObject as XElement;
            if (xElement != null)
                return xElement.Value;

            var xAttribute = xObject as XAttribute;
            if (xAttribute != null)
                return xAttribute.Value;

            var xText = xObject as XText;
            if (xText != null)
                return xText.Value;

            throw new Exception(string.Format("Cannot get string value from '{0}'", xObject));
        }

        public static bool GetAsBoolean(this XObject xObject)
        {
            var value = xObject.GetAsString();

            if (value == "1")
                return true;

            if (value == "0")
                return false;

            return bool.Parse(value);
        }

        public static byte GetAsByte(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return byte.Parse(value);
        }

        public static short GetAsShort(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return short.Parse(value);
        }

        public static int GetAsInt(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return int.Parse(value);
        }

        public static long GetAsLong(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return long.Parse(value);
        }

        public static Type GetAsType(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return Type.GetType(value, true);
        }

        public static Guid GetAsGuid(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return Guid.Parse(value);
        }

        public static T GetAsEnum<T>(this XObject xObject)
        {
            var value = xObject.GetAsString();

            return (T)Enum.Parse(typeof(T), value);
        }

        public static bool TryGetElement(this XElement xElement, XName name, out XElement element)
        {
            element = xElement.Element(name);

            return element != null;
        }

        public static bool TryGetAttribute(this XElement xElement, XName name, out XAttribute attribute)
        {
            attribute = xElement.Attribute(name);

            return attribute != null;
        }

        public static XElement SubElement(this XElement xElement)
        {
            return xElement.Elements().First();
        }
    }
}
