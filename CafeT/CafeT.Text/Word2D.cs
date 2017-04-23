using System;

namespace CafeT.Text
{
    public class Word2D
    {
        public int Index { set; get; }
        public string Value { set; get; }

        public Word2D(int i, string value)
        {
            Index = i;
            Value = value;
        }
        public override string ToString()
        {
            return Value + "[" + Index.ToString() + "]";
        }
    }
}
