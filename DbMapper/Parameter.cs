using System;

namespace DbMapper
{
    public class Parameter
    {
        public static Parameter Next
        {
            get { return new Parameter(); }
        }

        public Type Type { get; private set; }

        internal string Name;
        internal int Index;

        public static implicit operator Parameter(Type x)
        {
            return new Parameter { Type = typeof(Type) };
        }

        public static implicit operator Parameter(string x)
        {
            return new Parameter { Type = typeof(string) };
        }

        public static implicit operator Parameter(char x)
        {
            return new Parameter { Type = typeof(char) };
        }

        public static implicit operator Parameter(DateTime x)
        {
            return new Parameter { Type = typeof(DateTime) };
        }

        public static implicit operator Parameter(bool x)
        {
            return new Parameter { Type = typeof(bool) };
        }

        public static implicit operator Parameter(byte x)
        {
            return new Parameter { Type = typeof(byte) };
        }

        public static implicit operator Parameter(short x)
        {
            return new Parameter { Type = typeof(short) };
        }

        public static implicit operator Parameter(int x)
        {
            return new Parameter { Type = typeof(int) };
        }

        public static implicit operator Parameter(long x)
        {
            return new Parameter { Type = typeof(long) };
        }

        public static implicit operator Parameter(float x)
        {
            return new Parameter { Type = typeof(float) };
        }

        public static implicit operator Parameter(double x)
        {
            return new Parameter { Type = typeof(double) };
        }

        public static implicit operator Parameter(decimal x)
        {
            return new Parameter { Type = typeof(decimal) };
        }

        public static implicit operator Parameter(Enum x)
        {
            return new Parameter { Type = typeof(Enum) };
        }

        public static implicit operator Parameter(char? x)
        {
            return new Parameter { Type = typeof(char?) };
        }

        public static implicit operator Parameter(DateTime? x)
        {
            return new Parameter { Type = typeof(DateTime?) };
        }

        public static implicit operator Parameter(bool? x)
        {
            return new Parameter { Type = typeof(bool?) };
        }

        public static implicit operator Parameter(byte? x)
        {
            return new Parameter { Type = typeof(byte?) };
        }

        public static implicit operator Parameter(short? x)
        {
            return new Parameter { Type = typeof(short?) };
        }

        public static implicit operator Parameter(int? x)
        {
            return new Parameter { Type = typeof(int?) };
        }

        public static implicit operator Parameter(long? x)
        {
            return new Parameter { Type = typeof(long?) };
        }

        public static implicit operator Parameter(float? x)
        {
            return new Parameter { Type = typeof(float?) };
        }

        public static implicit operator Parameter(double? x)
        {
            return new Parameter { Type = typeof(double?) };
        }

        public static implicit operator Parameter(decimal? x)
        {
            return new Parameter { Type = typeof(decimal?) };
        }

        public static bool operator ==(Parameter p1, Parameter p2)
        {
            return true;
        }

        public static bool operator !=(Parameter p1, Parameter p2)
        {
            return true;
        }

        public static bool operator >(Parameter p1, Parameter p2)
        {
            return true;
        }

        public static bool operator <(Parameter p1, Parameter p2)
        {
            return true;
        }
    }
}
