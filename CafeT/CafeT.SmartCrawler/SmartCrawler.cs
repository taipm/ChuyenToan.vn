using CafeT.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using CafeT.Text;
using System.IO;
using CafeT.Objects;
using CafeT.SmartObjects;

namespace CafeT.SmartCrawler
{
    public interface ISmartCrawler
    {
        bool CheckContent();
        void GetNextUrls();
        void Run();
        bool HasMeaning(string url);
    }

    public class SmartCrawler:ISmartCrawler
    {
        public string Url { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public string TextDb { set; get; }
        public string OutputFile { set; get; }
        public int Depth { set; get; }
        public int Step { set; get; }

        public int CountRun { set; get; }

        public DateTime StartRunTime { set; get; }
        public DateTime EndRunTime { set; get; }
        public List<DateTime> Schedules { set; get; }

        public string[] UrlKeyWords { set; get; }

        public string[] IgnoreKeywords { set; get; }
        protected List<string> NextUrls { set; get; }

        public SmartHtml HtmlSmart { set; get; }
        public SmartUrl UrlSmart { set; get; }

        public WebPage Page { set; get; }
        
        public SmartCrawler(string url)
        {
            if(url.IsUrl())
            {
                UrlSmart = new SmartUrl(url);
                Page = new WebPage(url);
                Depth = 0;
                Name = url;
                CountRun = 0;
                InitSchedules();
            }
            else
            {
                throw new Exception("url is not valid");
            }
        }

        public void InitSchedules()
        {
            if(Schedules != null)
            {
                Schedules = new List<DateTime>();
            }
            for(int i=24; i ==1; i--)
            {
                if(i%2==0)
                {
                    Schedules.Add(DateTime.Parse(i.ToString() + ":00"));
                }
            }
        }

        public void ClearSchedules()
        {
            Schedules = new List<DateTime>();
        }

        public void AddSchedules(DateTime time)
        {
            Schedules.Add(time);
            Schedules = Schedules.Distinct().ToList();
        }

        public SmartCrawler(string crawlerTemplate, string crawlerOutput)
        {
            SmartFile _file = new SmartFile(crawlerTemplate);
            string[] _items = _file.Lines[0].Split(new string[] { "|" }, StringSplitOptions.None);
            Name = _items[0]; //CrawlerName
            Url = _items[1]; //Url
            OutputFile = _items[2];
            Depth = int.Parse(_items[3]);
            UrlKeyWords = _items[4].Split(new string[] { ";" }, StringSplitOptions.None).Where(t=>t.Length>0).ToArray();
            InitSchedules();
            CountRun = 0;
            Step = 0;

            if (! string.IsNullOrWhiteSpace(_items[5]))
            {
                IgnoreKeywords = _items[5].Split(new string[] { ";" }, StringSplitOptions.None).Where(t => t.Length > 0).ToArray();
            }

            if (OutputFile == null || OutputFile == string.Empty)
            {
                OutputFile = @"C:\TmpC#\Crawlers\" + Name + ".txt";
            }

            UrlSmart = new SmartUrl(Url);
            Page = new WebPage(Url);

            OutputFile = crawlerOutput + OutputFile;

            if (!File.Exists(OutputFile))
            {
                File.Create(OutputFile);
                File.WriteAllText(OutputFile, "Crawler " + Name);
            }
            AddResult();

            NextUrls = new List<string>();
            NextUrls = Page.HtmlSmart.InternalLinks.Where(t => t.IsUrl() && IsAccept(t)).Distinct().ToList();
        }

        public SmartCrawler(string name, string url, string pathOutput, string[] urlAcceptWords, string[] ignoreKeywords, int dept)
        {
            Name = name;
            Url = url;
            CountRun = 0;
            OutputFile = pathOutput;
            UrlKeyWords = urlAcceptWords;
            Depth = dept;
            Step = 0;
            IgnoreKeywords = ignoreKeywords;
            InitSchedules();
            UrlSmart = new SmartUrl(Url);
            Page = new WebPage(Url);

            if (urlAcceptWords.Length <= 0)
            {
                UrlKeyWords.ToList().Add(Url);
            }

            if (pathOutput == null || pathOutput == string.Empty)
            {
                pathOutput = @"C:\TmpC#\Crawlers\" + Name + ".txt";
            }

            AddResult();

            GetNextUrls();
        }

        private void AddResult()
        {
            SmartFile _file = new SmartFile(OutputFile);

            if (IsAccept(Url) && !IsExits(Url))
            {
                Page = new WebPage(Url);
                DateTime _time = DateTime.Now;
                _file.AddLineBefore("{" + _time + "}" + "{Title}{" + Page.Title.ToStandard() + "}");
                _file.Save();
                _file.AddLineBefore("{" + _time + "}" + "{Url}{" + Url + "}");
                _file.Save();
            }
        }

        public bool IsEnable()
        {
            DateTime _now = DateTime.Now;
            foreach (var _time in Schedules)
            {
                if(_now == _time)
                {
                    return true;
                }
            }
            return false;
        }

        public void GetNextUrls()
        {
            NextUrls = new List<string>();
            try
            {
                NextUrls = Page.InternalLinks.Where(t => t.IsUrl() && IsAccept(t)).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadCrawlerDb()
        {
            SmartFile _outFile = new SmartFile(OutputFile);
            TextDb = _outFile.Content;
        }

        
        public void Run()
        {
            if (CountRun == 0 || this.IsEnable())
            {
                LoadCrawlerDb();
                while (NextUrls != null && NextUrls.Count > 0 && !IsStopCrawler())
                {
                    foreach (string url in NextUrls)
                    {
                        if (IsAccept(url))
                        {
                            new SmartCrawler(Name, url, OutputFile, UrlKeyWords, IgnoreKeywords, 0);
                        }
                    }
                    Step = Step + 1;
                }
                CountRun = CountRun + 1;
            }
            else
            {
                Console.WriteLine("Can't run at this time {" + DateTime.Now + "}");
            }
        }

        public bool HasMeaning(string url)
        {
            if(url.ContainsAny(UrlKeyWords) && !url.ContainsAny(IgnoreKeywords))
            {
                return true;
            }
            return false;
        }

        public bool CheckContent()
        {
            string _text = Page.HtmlContent.HtmlToText();
            SmartText _smartText = new SmartText(_text);
            if (!_smartText.IsValid()) return false;
            if (_smartText.HasError) return false;
            if (_smartText.Words.Length < 100) return false;
            return true;
        }

        public bool IsAccept(string url)
        {
            if(HasMeaning(url) && CheckContent())
            {
                return true;
            }
            return false;
        }

        public bool IsExits(string url)
        {
            SmartFile _smartDb = new SmartFile(OutputFile);
            if(_smartDb.Lines != null && _smartDb.Lines.Count()> 0&& _smartDb.Lines.Contains(url))
            {
                return true;
            }
            return false;
        }

        public bool IsStopCrawler()
        {
            if (Step > Depth) return true;
            return false;
        }

        public override string ToString()
        {
            return this.PrintAllProperties();
        }
    }
}
