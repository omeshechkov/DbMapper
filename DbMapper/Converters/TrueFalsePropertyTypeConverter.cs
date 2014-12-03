using System;

namespace DbMapper.Converters
{
    public class TrueFalseConverter : Converter<string, bool>
    {
        protected override Func<string, bool> ConverterFromT1ToT2
        {
            get { return v => v == "T"; }
        }

        protected override Func<bool, string> ConverterFromT2ToT1
        {
            get { return v => v ? "T" : "F"; }
        }
    }

    public class LowerTrueFalseConverter : Converter<string, bool>
    {
        protected override Func<string, bool> ConverterFromT1ToT2
        {
            get { return v => v == "t"; }
        }

        protected override Func<bool, string> ConverterFromT2ToT1
        {
            get { return v => v ? "t" : "f"; }
        }
    }
}