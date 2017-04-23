using AI.TextMining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextMining
{
    public static class MapText
    {
        public static Map ToMap(this string input)
        {
            input = input.ToStandard();
            Map _map = new Map();
           
            string[] _words = input.ToWords();
            for(int i = 0; i < _words.Length; i++)
            {
                MapItem _item = new MapItem();
                _item.Value = _words[i];
                _item.Index = i;
                _map.Items.Add(_item);
            }
            return _map;
        }

        

        public static string[] ToWords(this string input)
        {
            input = input.HtmlToText().ToStandard();
            List<string> _words = new List<string>();
            while(input.HasWord())
            {
                string _firstWord = input.FirstWord();
                _words.Add(_firstWord);
                input = input.Replace(_firstWord, "").ToStandard();
            }
            return _words.ToArray();
        }

        public static string FirstWord(this string input)
        {
            input = input.ToStandard();
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else if (!input.Contains(" "))
            {
                return input;
            }
            else
            {
                int _index = input.IndexOf(" ");
                return input.Substring(0, _index);
            }
        }

        public static bool HasWord(this string input)
        {
            if (string.IsNullOrEmpty(input.FirstWord())) return false;
            return true;
        }

        public static Map TextToMap(this string input)
        {
            Map _map = new Map();
            
            return _map;
        }
    }

    public class Map
    {
        public List<MapItem> Items { set; get; }
        public Map()
        {
            Items = new List<MapItem>();
        }

        public MapItem First()
        {
            return Items.Where(c => c.Index == 0).FirstOrDefault();
        }
        public MapItem Last()
        {
            return Items.Where(c => c.Index == Items.Count).FirstOrDefault();
        }

        public string ToText()
        {
            string _text = string.Empty;
            foreach(var item in Items)
            {
                _text += _text + " " + item.Value;
            }
            return _text;
        }

    }

    public class MapItem
    {
        public string Value { set; get; }
        public int Index { set; get; }
        //public MapItem After { set; get; }
        //public MapItem Before { set; get; }

        public MapItem()
        {
            Value = string.Empty;
            Index = 0;
            //After = new MapItem();
            //After.Index = this.Index + 1;

            //Before = new MapItem();
            //Before.Index = this.Index - 1;
        }

        public override string ToString()
        {
            return Value + " | " + Index.ToString();

        }
    }
}
