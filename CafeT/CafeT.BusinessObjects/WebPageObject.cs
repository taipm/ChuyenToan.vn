using CafeT.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects
{
    public class WebPageObject : BaseObject
    {
        [NotMapped]
        public WebPage Page { set; get; }
        
        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }
        
        public string[] NodesCss { set; get; }

        public WebPageObject() : base()
        {
            Title = string.Empty;
            Url = string.Empty;
            Page = new WebPage();
        }
        public WebPageObject(string url) : base()
        {
            Page = new WebPage(url);
            Title = Page.Title;
            Url = url;
            HtmlContent = Page.HtmlContent;
            //Smart = Page.Smart;
        }

        //public void LoadTitle()
        //{
        //    Title = string.Empty;
        //}

        //public void LoadHtml()
        //{
        //    // The HtmlWeb class is a utility class to get the HTML over HTTP
        //    HtmlWeb htmlWeb = new HtmlWeb();

        //    // Creates an HtmlDocument object from an URL
        //    Document = htmlWeb.Load(Url);

        //    HtmlContent = Document.DocumentNode.OuterHtml;
        //}
        //public string[] GetLinks()
        //{
        //    List<string> _links = new List<string>();
        //    HtmlNode[] nodes = Document.DocumentNode.SelectNodes("//a").ToArray();
        //    foreach (HtmlNode item in nodes)
        //    {
        //        _links.Add(item.InnerHtml);
        //    }
        //    return _links.ToArray();
        //}

        //List<string> _Links = new List<string>();
        //int i = 0;
        //public void GetNodesCss()
        //{
        //    if (!Smart.Document.HasChildNodes)
        //    {
        //        var _text = "<" + node.OuterHtml.MinBetween("<", ">") + ">";

        //        _Links.Add(_text);
        //    }
        //    else
        //    {
        //        while (node.HasChildNodes)
        //        {
        //            i = i + 1;
        //            foreach (HtmlNode _x in node.ChildNodes)
        //            {
        //                GetNodesCss(_x);
        //                node = _x;

        //            }

        //        }
        //    }

        //}

        //public string[] GetNodesCss0()
        //{
        //    List<string> _links = new List<string>();
        //    HtmlNode _node = Document.DocumentNode;

        //    while (_node.HasChildNodes)
        //    {
        //        _links.AddRange(_node.ChildNodes.Select(t => t.Name).ToList());
        //        foreach (HtmlNode _x in _node.ChildNodes)
        //        {
        //            _links.AddRange(_node.ChildNodes.Select(t => t.Name).ToList());
        //        }
        //        //_node = 
        //    }

        //    return _links.ToArray();
        //}
        //public string GetHtmlContentByClass(string className)
        //{
        //    return string.Empty;
        //}
        //public void GetNodeBy(string className)
        //{
        //    var allElementsWithClassFloat = Document.DocumentNode.SelectNodes("//*[contains(@class,'float')]");
        //}
    }
}
