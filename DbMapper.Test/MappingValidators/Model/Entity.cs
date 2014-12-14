namespace DbMapper.Test.MappingValidators.Model
{
    internal class Entity
    {
        private int _val;

        public static int StaticField;

        public int NoSetter
        {
            get { return _val; }
        }

        public int NoGetter
        {
            set { _val = value; }
        }

        public int GetValue()
        {
            return 0;
        }

        public long Id { get; set; }

        public object Value { get; set; }
    }
}