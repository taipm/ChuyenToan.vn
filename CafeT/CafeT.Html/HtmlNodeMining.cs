using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeT.DataMining;

namespace CafeT.DataMining
{
    public static class HtmlNodeMining
    {
        public static List<HtmlNode> GetHtmlNodes(this HtmlNode root)
        {
            List<HtmlNode> _nodes = new List<HtmlNode>();

            if (root == null) return null;
            while (root.HasChildNodes)
            {
                foreach (var _node in root.ChildNodes)
                {
                    _nodes.Add(_node);
                    root = _node;
                }
                GetHtmlNodes(root);
            }
            return _nodes;
        }

        public static List<string> GetNodes(this HtmlNode root)
        {
            Dictionary<string, string> _nodes = new Dictionary<string, string>();
            List<string> _nodesStr = new List<string>();

            if (root == null) return null;
            while (root.HasChildNodes)
            {
                foreach (var _node in root.ChildNodes)
                {
                    var _child = _node;
                    _nodesStr.Add(_child.Name + "|" + _child.OuterHtml + "<br />");
                    root = _node;
                }
                GetNodes(root);
            }
            return _nodesStr;
        }

        public static List<string> GetNodes(this HtmlNode root, bool? removeEmty)
        {
            Dictionary<string, string> _nodes = new Dictionary<string, string>();
            List<string> _nodesStr = new List<string>();

            if (root == null) return null;
            while (root.HasChildNodes)
            {
                foreach (var _node in root.ChildNodes)
                {
                    var _child = _node;
                    if (removeEmty.HasValue && removeEmty.Value)
                    {
                        if (_child.OuterHtml.HtmlToText().ToStandard().Length > 2)
                            _nodesStr.Add(_child.XPath + "|" + _child.Name + "|" + _child.OuterHtml + "<br />");
                    }
                    else
                    {
                        _nodesStr.Add(_child.XPath + "|" + _child.Name + "|" + _child.OuterHtml + "<br />");
                    }

                    root = _node;
                }
                GetNodes(root);
            }
            return _nodesStr;
        }

        

        public static string GetContent(this HtmlNode node)
        {
            return node.OuterHtml;
        }

        public static string GetTitle(this HtmlNode node)
        {
            string _url = string.Empty;
            var _links = node.InnerHtml.GetNodeLinks();
            string _str1 = node.InnerText.ToStandard();
            Dictionary<string, string> _lst = new Dictionary<string, string>();
            if (_links != null && _links.Count()>0)
            {
                var _nodes = _links
                    .Where(c => (c != null)
                                && (c.OuterHtml != null)
                                && (c.OuterHtml.HtmlToText() != null)
                                && (c.OuterHtml.HtmlToText().Length > 0));
                foreach(var _node in _nodes)
                {
                    string _str2 = _node.OuterHtml.HtmlToText().ToStandard();
                    string[] _words = _str1.GetIntersectWords(_str2);
                    if (!_lst.Keys.Contains(_str2))
                        _lst.Add(_str2, _words.ToString());
                }
                var _item = _lst.Distinct().OrderByDescending(c => c.Value).FirstOrDefault();
                
                return _item.Key;
            }
            return _url;
        }

        public static string GetUrl(this HtmlNode node)
        {
            string _url = string.Empty;
            var _links = node.InnerHtml.GetNodeLinks();
            string _str1 = node.InnerText.ToStandard();
            Dictionary<string, string> _lst = new Dictionary<string, string>();
            if (_links != null && _links.Count() > 0)
            {
                var _nodes = _links
                    .Where(c => (c != null)
                                && (c.OuterHtml != null)
                                && (c.OuterHtml.HtmlToText() != null)
                                && (c.OuterHtml.HtmlToText().Length > 0));
                foreach (var _node in _nodes)
                {
                    string _str2 = _node.OuterHtml.GetLinks().FirstOrDefault() + "|" +  _node.OuterHtml.HtmlToText().ToStandard();
                    string[] _words = _str1.GetIntersectWords(_str2);
                    if(!_lst.Keys.Contains(_str2))
                        _lst.Add(_str2, _words.ToString());
                }
                var _item = _lst.Distinct().OrderByDescending(c => c.Value).FirstOrDefault();
                if(_item.Key.Contains("|"))
                {
                    return _item.Key.Substring(0, _item.Key.IndexOf("|"));
                }
            }
            return _url;
        }

        public static GenericArticle HtmlNodeToGArticle(this HtmlNode node, GenericUrl url, string headerUrl)
        {
            string _cssTitle = url.TitleCss;
            string _cssUrl = url.UrlCss;
            string _cssDescription = url.DescriptionCss;

            GenericArticle _article = new GenericArticle();

            string _tmpUrl = string.Empty;

            _article.Url = node.GetUrl();
            if (_article.Url != null && !_article.Url.IsUrl()) return null;

            _article.Title = node.GetTitle();

            if (url.DescriptionCss != null)
                _article.Summary = node.GetNodesByClasses(_cssDescription).FirstOrDefault().OuterHtml;

            _article.Content = node.GetContent();
            
            if (!_article.IsGood()) return null;
            return _article;
        }
    }
}
