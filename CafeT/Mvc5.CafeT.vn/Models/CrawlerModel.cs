using CafeT.BusinessObjects;
using CafeT.SmartCrawler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class CrawlerModel:BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public int Depth { set; get; }

        [Required]
        public string Url { set; get; }
        public string PathConfig { set; get; }
        public string PathOutput { set; get; }
        public string AcceptKeywords { set; get; }
        public string RejectKeywords { set; get; }

        public DateTime[] Schedules { set; get; }

        public bool Enable { set; get; }

        public DateTime? LastestRun { set; get; }

        public CrawlerModel():base()
        {
            Depth = 0;
            Enable = false;
        }

        public SmartCrawler ToSmartCrawler()
        {
            SmartCrawler _crawler = new SmartCrawler(Url);
            return _crawler;
        }

        public void ToEnable()
        {
            Enable = true;
        }
    }

    //public class SmartCrawler : ISmartCrawler
    //{
    //    public string Url { set; get; }

    //    public string Name { set; get; }
    //    public string Description { set; get; }

    //    public string TextDb { set; get; }
    //    public string OutputFile { set; get; }
    //    public int Depth { set; get; }
    //    public int Step { set; get; }

    //    public int CountRun { set; get; }

    //    public DateTime StartRunTime { set; get; }
    //    public DateTime EndRunTime { set; get; }

    //    public string[] UrlKeyWords { set; get; }

    //    public string[] IgnoreKeywords { set; get; }
    //    protected List<string> NextUrls { set; get; }

    //    public SmartHtml HtmlSmart { set; get; }
    //    public SmartUrl UrlSmart { set; get; }

    //    public WebPage Page { set; get; }

    //    public SmartCrawler(string url)
    //    {
    //        if (url.IsUrl())
    //        {
    //            UrlSmart = new SmartUrl(url);
    //            Page = new WebPage(url);
    //            Depth = 0;
    //            Name = url;
    //            CountRun = 0;
    //        }
    //        else
    //        {
    //            throw new Exception("url is not valid");
    //        }
    //    }

    //    public SmartCrawler(string crawlerTemplate, string crawlerOutput)
    //    {
    //        SmartFile _file = new SmartFile(crawlerTemplate);
    //        string[] _items = _file.Lines[0].Split(new string[] { "|" }, StringSplitOptions.None);
    //        Name = _items[0]; //CrawlerName
    //        Url = _items[1]; //Url
    //        OutputFile = _items[2];
    //        Depth = int.Parse(_items[3]);
    //        UrlKeyWords = _items[4].Split(new string[] { ";" }, StringSplitOptions.None).Where(t => t.Length > 0).ToArray();

    //        CountRun = 0;
    //        Step = 0;

    //        if (!string.IsNullOrWhiteSpace(_items[5]))
    //        {
    //            IgnoreKeywords = _items[5].Split(new string[] { ";" }, StringSplitOptions.None).Where(t => t.Length > 0).ToArray();
    //        }

    //        if (OutputFile == null || OutputFile == string.Empty)
    //        {
    //            OutputFile = @"C:\TmpC#\Crawlers\" + Name + ".txt";
    //        }

    //        UrlSmart = new SmartUrl(Url);
    //        Page = new WebPage(Url);

    //        OutputFile = crawlerOutput + OutputFile;

    //        if (!File.Exists(OutputFile))
    //        {
    //            File.Create(OutputFile);
    //            File.WriteAllText(OutputFile, "Crawler " + Name);
    //        }

    //        SmartFile _outFile = new SmartFile(OutputFile);
    //        if (IsAccept(Url))
    //        {
    //            Page = new WebPage(Url);
    //            _file.AddLineBefore(Page.Title.ToStandard());
    //            _file.Save();
    //            _file.AddLineBefore(Url);
    //            _file.Save();
    //        }

    //        //if (HasMeaning(Url) && !_outFile.Lines.Contains(Url))
    //        //{
    //        //    _outFile.AddLineBefore(Url);
    //        //    _outFile.Save();
    //        //}

    //        NextUrls = new List<string>();
    //        NextUrls = Page.HtmlSmart.InternalLinks.Where(t => t.IsUrl() && IsAccept(t)).Distinct().ToList();
    //    }

    //    public SmartCrawler(string name, string url, string pathOutput, string[] urlAcceptWords, string[] ignoreKeywords, int dept)
    //    {
    //        Name = name;
    //        Url = url;
    //        CountRun = 0;
    //        OutputFile = pathOutput;
    //        UrlKeyWords = urlAcceptWords;
    //        Depth = dept;
    //        Step = 0;
    //        IgnoreKeywords = ignoreKeywords;

    //        UrlSmart = new SmartUrl(Url);
    //        Page = new WebPage(Url);

    //        if (urlAcceptWords.Length <= 0)
    //        {
    //            UrlKeyWords.ToList().Add(Url);
    //        }

    //        if (pathOutput == null || pathOutput == string.Empty)
    //        {
    //            pathOutput = @"C:\TmpC#\Crawlers\" + Name + ".txt";
    //        }

    //        SmartFile _file = new SmartFile(OutputFile);

    //        if (IsAccept(Url) && !IsExits(Url))
    //        {
    //            Page = new WebPage(Url);
    //            _file.AddLineBefore(Page.Title.ToStandard());
    //            _file.Save();
    //            _file.AddLineBefore(Url);
    //            _file.Save();
    //        }

    //        GetNextUrls();
    //    }

    //    public void GetNextUrls()
    //    {
    //        NextUrls = new List<string>();
    //        try
    //        {
    //            NextUrls = Page.InternalLinks.Where(t => t.IsUrl() && IsAccept(t)).Distinct().ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }
    //    }

    //    public void LoadCrawlerDb()
    //    {
    //        SmartFile _outFile = new SmartFile(OutputFile);
    //        TextDb = _outFile.Content;
    //    }


    //    public void Run()
    //    {
    //        LoadCrawlerDb();
    //        while (NextUrls != null && NextUrls.Count > 0 && !IsStopCrawler())
    //        {
    //            foreach (string url in NextUrls)
    //            {
    //                if (IsAccept(url))
    //                {
    //                    new SmartCrawler(Name, url, OutputFile, UrlKeyWords, IgnoreKeywords, 0);
    //                }
    //            }
    //            Step = Step + 1;
    //        }
    //        CountRun = CountRun + 1;
    //    }

    //    public bool HasMeaning(string url)
    //    {
    //        if (url.ContainsAny(UrlKeyWords) && !url.ContainsAny(IgnoreKeywords))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    public bool CheckContent()
    //    {
    //        string _text = Page.HtmlContent.HtmlToText();
    //        SmartText _smartText = new SmartText(_text);
    //        if (!_smartText.IsValid()) return false;
    //        if (_smartText.HasError) return false;
    //        if (_smartText.Words.Length < 100) return false;
    //        return true;
    //    }

    //    public bool IsAccept(string url)
    //    {
    //        if (HasMeaning(url) && CheckContent())
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    public bool IsExits(string url)
    //    {
    //        SmartFile _smartDb = new SmartFile(OutputFile);
    //        if (_smartDb.Lines != null && _smartDb.Lines.Count() > 0 && _smartDb.Lines.Contains(url))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    public bool IsStopCrawler()
    //    {
    //        if (Step > Depth) return true;
    //        return false;
    //    }

    //    public override string ToString()
    //    {
    //        return this.PrintAllProperties();
    //    }
    //}
}