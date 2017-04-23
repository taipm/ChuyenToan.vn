using CafeT.Objects;
using CafeT.Text;
using CafeT.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeT.SmartObjects
{
    public class SmartText
    {
        public bool HasError { set; get; }
        public string Text { set; get; }
        public string OutputText { set; get; }

        public Dictionary<int, string> Lines { set; get; }

        public string FirstWord { set; get; }
        public string LastWord { set; get; }
        public string[] Words { set; get; }
        public string[] Emails { set; get; }
        public string[] Urls { set; get; }
        public string[] Latexs { set; get; }

        public string[] YouTubeUrls { set; get; }
        public string[] GoogleDriveUrls { set; get; }

        public string[] Images { set; get; }
        public string[] Numbers { set; get; }
        public string[] Sentences { set; get; }
        public string[] Paragraphs { set; get; }
        public char[] Separators { set; get; }

        public SmartText(string text)
        {
            Text = text.ToStandard();
            if (string.IsNullOrWhiteSpace(text))
            {
                HasError = false;
            }
            else
            {
                try
                {
                    Words = Text.ToWords();
                    Separators = Text.ExtractSeparators();
                    Emails = Text.ExtractEmails();
                    Sentences = Text.ToSentences();
                    Numbers = Text.ExtractNumbers();
                    Urls = Text.ExtractUrls();
                    YouTubeUrls = Text.ExtractToYouTubeUrls();
                    GoogleDriveUrls = Text.ExtractToGoogleDriveUrls();
                    Latexs = Text.ExtractAllMathText();

                    if (Words != null && Words.Count() > 0)
                    {
                        FirstWord = Words[0];
                        LastWord = Words[Words.Length - 1];
                    }
                }
                catch (Exception ex)
                {
                    HasError = true;
                    throw new Exception("Can't convert this text to SmartText" + ex.Message);
                }
            }

        }

        public void ToLatex()
        {
            string[] _words = Text.ToWords();
            List<WordObject> _wordObjects = new List<WordObject>();
            if (_words != null && _words.Length > 0)
            {
                foreach(string _word in _words)
                {
                    WordObject _w = new WordObject(_word);
                    _w.ToLatex();
                    OutputText = OutputText.AddAfter(_w.Value + " ");
                }
            }
        }

        public bool IsValid()
        {
            if (Words == null || Words.Count() <= 0) return false;
            return true;
        }

        public void AddBefore(string text)
        {
            Text.AddBefore(text);
        }

        public void AddAfter(string text)
        {
            Text.AddAfter(text);
        }

        public Dictionary<int, string> GetLines()
        {
            Dictionary<int, string> _dictLines = new Dictionary<int, string>();

            using (StringReader reader = new StringReader(Text))
            {
                int lineNo = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _dictLines.Add(++lineNo, line);
                }
            }
            return _dictLines;
        }
       
        public string[] GetSentences(string word)
        {
            return this.Text.GetSentences(word);
        }

        public string NextWord(string word)
        {
            for(int i = 0; i <= Words.Length -1; i++)
            {
                if (Words[i] == word) return Words[i + 1];
            }
            return string.Empty;
        }

        public string PrevWord(string word)
        {
            for (int i = 0; i <= Words.Length - 2; i++)
            {
                if (Words[i+1] == word) return Words[i];
            }
            return string.Empty;
        }

        public void Save(string path)
        {
            Writer _writer = new Writer(path);
            _writer.WriteToText(path, this.ToString());
        }

        public override string ToString()
        {
            return this.PrintAllProperties();
        }
    }
}
