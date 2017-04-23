using CafeT.SmartFiles;
using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Html
{
    public class WebPage
    {
        public SmartHtml HtmlSmart { set; get; }
        public WebPageCssConfig CssConfig { set; get; }
        public string PathConfig { set; get; }
        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }
        //public string TextContent { set; get; }
        public string[] InternalLinks { set; get; }
        public string[] ExternalLinks { set; get; }

        HtmlParseError Errors { set; get; }

        public string[] NodesCss { set; get; }

        public WebPage() 
        {
            Title = string.Empty;
        }

        public WebPage(string url)
        {
            if(url.IsUrl())
            {
                Url = url;
                HtmlContent = LoadHtml(url);
                HtmlSmart = new SmartHtml(HtmlContent);
                InternalLinks = HtmlSmart.InternalLinks;
                ExternalLinks = HtmlSmart.ExternalLinks;
                LoadTitle();
                LoadContent();

                List<string> _newInternalLinks = new List<string>();
                foreach (string _link in InternalLinks)
                {

                    if (_link.StartsWith("/"))
                    {
                        string _fullUrl = Url + _link;
                        _newInternalLinks.Add(_fullUrl);
                        InternalLinks.ToList().Remove(_link);
                        InternalLinks.ToList().Add(_fullUrl);
                    }
                }
            }

            
            //if(_newInternalLinks.Count > 0)
            //{
            //    foreach(string _newLink in _newInternalLinks)
            //    {
            //        InternalLinks.de
            //    }
            //    InternalLinks = _newInternalLinks.ToArray();
            //}
        }
        public void Load()
        {
            HtmlSmart = new SmartHtml(LoadHtml(Url));
        }
        public void LoadConfig()
        {
            CssConfig = new WebPageCssConfig(PathConfig);
        }

        public void LoadContent()
        {
            string _minTag = HtmlSmart.MeaningNodes
                .Where(t => t.InnerText.ToStandard().GetCountWords() > 100)
                .Select(t => t.OuterHtml).FirstOrDefault();
            HtmlContent = _minTag;
        }
        public void LoadTitle()
        {
            if (HtmlSmart.Nodes == null || HtmlSmart.Nodes.Count() == 0)
            {
                Title = string.Empty;
            }
            string _minTag = HtmlSmart.Nodes.Where(t=>t.CanTitle())
                .Where(t => t.InnerText.ToStandard().GetCountWords()> 5)
                .Select(t => t.InnerText.ToStandard()).OrderBy(t => t.GetCountWords()).FirstOrDefault();
            Title = _minTag;
        }

        public string LoadHtml(string url)
        {
            if(url.IsUrl() && !url.IsImageUrl())
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                try
                {
                    return htmlWeb.Load(url).DocumentNode.OuterHtml;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public string GetHtmlContentByClass(string className)
        {
            return string.Empty;
        }

        public void GetNodeBy(string className)
        {
            var allElementsWithClassFloat = HtmlSmart.DocumentNode.SelectNodes("//*[contains(@class,'float')]");
        }
    }
}
