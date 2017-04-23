using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public class WordGridItem
    {
        public int i { set; get; }
        public int j { set; get; }
        public string Word { set; get; }
    }

    public class WordGrid
    {
        public string Text { set; get; }
        public int m { set; get; } //row
        public int n { set; get; } //colunm

        List<WordGridItem> Items { set; get; }

        public WordGrid(string input, int colunm)
        {
            Items = new List<WordGridItem>();
            int _countOfWords = input.GetCountWords();
            if(_countOfWords <= colunm)
            {
                m = 0;
                n = _countOfWords;
            }
            else
            {
                n = colunm;
                m = _countOfWords / n + 1;
            }
            Text = input;
            Load();
        }

        public WordGrid(string fileName)
        {
            int colunm = 10; //default
            if (File.Exists(fileName))
            {
                Text = File.ReadAllText(fileName);
                Items = new List<WordGridItem>();
                int _countOfWords = Text.GetCountWords();
                if (_countOfWords <= colunm)
                {
                    m = 0;
                    n = _countOfWords;
                }
                else
                {
                    n = colunm;
                    m = _countOfWords / n + 1;
                }
                Load();
            }
        }

        public WordGridItem GetItem(int i, int j)
        {
            try
            {
                return Items.ToArray()[(i - 1+1) * n + j];
            }
            catch
            {
                return null;
            }
        }

        public void Load()
        {
            string[] _words = Text.ToWords();
            if(_words != null && _words.Count()>0)
            {
                for(int i = 0; i<m; i++)
                {
                    for (int j = 0; j <n; j++)
                    {
                        try
                        {
                            WordGridItem _item = new WordGridItem();
                            _item.i = i;
                            _item.j = j;
                            _item.Word = _words[(i-1+1)*n + j];
                            Items.Add(_item);
                        }
                        catch
                        {
                            WordGridItem _item = new WordGridItem();
                            _item.i = i;
                            _item.j = j;
                            _item.Word = string.Empty;
                            Items.Add(_item);
                        }
                    }
                }
            }
        }

        public void Print(string fileName)
        {
            StreamWriter _writer = new StreamWriter(fileName);

            for (int i = 0; i< m; i++)
            {
                for(int j = 0; j <n; j++)
                {
                    if(this.GetItem(i, j) != null)
                    {
                        _writer.Write(this.GetItem(i, j).Word + " ");
                    }
                    
                }
                _writer.WriteLine();
            }
            _writer.Close();
        }

        public void PrintWithLatex(string fileName)
        {
            StreamWriter _writer = new StreamWriter(fileName);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (this.GetItem(i, j) != null)
                    {
                        _writer.Write("$" + this.GetItem(i, j).Word +"_"+ j.ToString() + "^" + i.ToString() + "$");
                    }

                }
                _writer.WriteLine();
            }
            _writer.Close();
        }
    }
}
