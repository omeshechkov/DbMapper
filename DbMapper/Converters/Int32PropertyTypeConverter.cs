using System;

namespace DbMapper.Converters
{
    public class Int32ByteConverter : Converter<int, byte>
    {
        protected override Func<int, byte> ConverterFromT1ToT2
        {
            get { return v => (byte)v; }
        }

        protected override Func<byte, int> ConverterFromT2ToT1
        {
            get { return v => (int)v; }
        }
    }

    public class Int32Int16Converter : Converter<int, short>
    {
        protected override Func<int, short> ConverterFromT1ToT2
        {
            get { return v => (short)v; }
        }

        protected override Func<short, int> ConverterFromT2ToT1
        {
            get { return v => (int)v; }
        }
    }
}