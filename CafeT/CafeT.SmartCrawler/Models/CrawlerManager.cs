using CafeT.Folders;
using CafeT.SmartObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartCrawler.Models
{
    public class CrawlerManager
    {
        public string CrawlerInputFolder { set; get; }
        public string CrawlerOutputFolder { set; get; }

        public List<SmartCrawler> Crawlers { set; get; }

        public CrawlerManager()
        {

        }
        /// <summary>
        /// crawlerFolder - No end with \
        /// crawlerOutputFolder - endWith \
        /// </summary>
        /// <param name="crawlerFolder"></param>
        /// <param name="crawlerOutputFolder"></param>
        public CrawlerManager(string crawlerFolder, string crawlerOutputFolder)
        {
            CrawlerInputFolder = crawlerFolder;
            CrawlerOutputFolder = crawlerOutputFolder;
            Crawlers = new List<SmartCrawler>();
            LoadCrawlers(CrawlerInputFolder);
        }

        public SmartCrawler LoadCrawler(string pathConfig)
        {
            SmartFile _smartFile = new SmartFile(pathConfig);
            string _line = _smartFile.Lines.FirstOrDefault();
            string[] _items = _line.Split(new string[] { "|" }, StringSplitOptions.None);
            string _crawlerName = _items[0].Trim();
            string _crawlerUrl = _items[1].Trim();
            string _crawlerOutput = _items[2].Trim().Trim();
            int _dept = int.Parse(_items[3].Trim());
            string[] _crawlerKeyWords = _items[4].Split(new string[] { ";" }, StringSplitOptions.None)
                .Where(t => t != null && t.Length > 0).ToArray();
            string[] _crawlerIgnoreKeyWords = _items[5].Split(new string[] { ";" }, StringSplitOptions.None)
                .Where(t => t != null && t.Length > 0).ToArray();

            var _crawler = new SmartCrawler(_crawlerName, _crawlerUrl, CrawlerOutputFolder + _crawlerOutput, _crawlerKeyWords, _crawlerIgnoreKeyWords, _dept);

            return _crawler;
        }
        /// <summary>
        /// All crawlers is configed as text file and put in crawlerFolder
        /// CrawlerName|rule for fetch url
        /// Name of Url | Url | Output | Dept| Keywords split by ";"
        /// Example for Crawler config
        /// VMFCrawler|http://diendantoanhoc.net/|VMF.txt|topic;
        /// </summary>
        /// <param name="crawlerFolder"></param>
        public void LoadCrawlers(string crawlerFolder)
        {
            SmartFolder _folder = new SmartFolder(crawlerFolder, false);
            var _smartFiles = _folder.GeSmartFiles(false);
            if(_smartFiles != null && _smartFiles.Count() > 0)
            {
                foreach(var smartFile in _smartFiles)
                {
                    var _crawler = LoadCrawler(smartFile.FullPath);
                    Crawlers.Add(_crawler);
                }
            }
        }

        public void Run()
        {
            if (Crawlers.Count > 0)
            {
                foreach (var crawler in Crawlers)
                {
                    crawler.Run();
                }
            }
        }
    }
}
