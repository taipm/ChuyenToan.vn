using System;
using System.Collections.Generic;

namespace CafeT.Text
{
    public class WordObject
    {
        public string Value { set; get; }
        public int Index { set; get; }
        public char CharBefore { set; get; }
        public char CharAfter { set; get; }
        public WordObject Before { set; get; }
        public WordObject After { set; get; }
        //public bool IsNumber { set; get; }
        public bool IsStart { set; get; }
        //public bool IsEnd { set; get; }
        public bool IsNoun { set; get; }
        public bool IsVerb { set; get; }
        public string Sentence { set; get; }

        public WordObject() { }
        public WordObject(string value)
        {
            
            if (String.IsNullOrEmpty(value) || value.Contains(" "))
            {
                throw new ArgumentException("Can't cast " + value + " to standard word");
            }
            else
            {
                Value = value;
                ToStandard();
            }
        }

        public bool IsLatex()
        {
            if (Value.StartsWith("$") && Value.EndsWith("$"))
                return true;
            return false;
        }

        public void ToLatex()
        {
            if(!this.IsLatex())
            {
                Value = "$" + Value + "$";
            }
        }

        public bool IsEnd()
        {
            if (Value.EndsWith(".") || Value.EndsWith("!") || Value.EndsWith("?")) return true;
            return false;
        }
        //public bool IsStart()
        //{
        //    if (Value.(".") || Value.EndsWith("!") || Value.EndsWith("?")) return true;
        //    return false;
        //}
        public void ToStandard()
        {
            Value = Value.ToStandard();
        }
    }

    public class DictionaryWord
    {
        public string OrignalWord { set; get; }
        public string TranslateWord { set; get; }
    }

    public class Sentence2D
    {
        public List<Word2D> Words { set; get; }
        public Sentence2D()
        {
            Words = new List<Word2D>();
        }
    }
}
