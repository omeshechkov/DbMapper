using System;
using System.Linq;
using System.Xml.Linq;

namespace DbMapper.Impl.Mappings.Xml.Utils
{
    public static class XmlUtils
    {
        private static string GetValueAsString(this XObject xObject)
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

        public static bool GetValueAsBoolean(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            if (value == "1")
                return true;

            if (value == "0")
                return false;

            return bool.Parse(value);
        }

        public static byte GetValueAsByte(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return byte.Parse(value);
        }

        public static short GetValueAsShort(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return short.Parse(value);
        }

        public static int GetValueAsInt(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return int.Parse(value);
        }

        public static long GetValueAsLong(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return long.Parse(value);
        }

        public static Type GetValueAsType(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return Type.GetType(value, true);
        }

        public static Guid GetValueAsGuid(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

            return Guid.Parse(value);
        }

        public static T GetValueAsEnum<T>(this XObject xObject)
        {
            var value = xObject.GetValueAsString();

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
            return xElement.Elements().FirstOrDefault();
        }
    }
}
