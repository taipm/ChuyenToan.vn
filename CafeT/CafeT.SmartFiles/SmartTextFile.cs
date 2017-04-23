using CafeT.SmartObjects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartFiles
{
    public class SmartTextFile
    {
        public SmartText Smart { set; get; }

        public SmartTextFile(string pathFile)
        {
            string Text = File.ReadAllText(pathFile);
            Smart = new SmartText(Text);
        }

        public string GetFirstLine(string word)
        {
            return Smart.Lines.First().Value;
        }
    }
}
