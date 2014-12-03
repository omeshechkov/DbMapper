using System;

namespace DbMapper.Converters
{
    public class SingleByteConverter : Converter<float, byte>
    {
        protected override Func<float, byte> ConverterFromT1ToT2
        {
            get { return v => (byte)v; }
        }

        protected override Func<byte, float> ConverterFromT2ToT1
        {
            get { return v => (float)v; }
        }
    }

    public class SingleInt16Converter : Converter<float, short>
    {
        protected override Func<float, short> ConverterFromT1ToT2
        {
            get { return v => (short)v; }
        }

        protected override Func<short, float> ConverterFromT2ToT1
        {
            get { return v => (float)v; }
        }
    }

    public class SingleInt32Converter : Converter<float, int>
    {
        protected override Func<float, int> ConverterFromT1ToT2
        {
            get { return v => (int)v; }
        }

        protected override Func<int, float> ConverterFromT2ToT1
        {
            get { return v => (float)v; }
        }
    }

    public class SingleInt64Converter : Converter<float, long>
    {
        protected override Func<float, long> ConverterFromT1ToT2
        {
            get { return v => (long)v; }
        }

        protected override Func<long, float> ConverterFromT2ToT1
        {
            get { return v => (float)v; }
        }
    }
}