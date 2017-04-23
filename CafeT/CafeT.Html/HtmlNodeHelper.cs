using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeT.Text;

namespace CafeT.Html
{
    
    public class SmartHtmlNode
    {
        public HtmlNode Node { set; get; }
        public SmartHtmlNode()
        {

        }

        public SmartHtmlNode(HtmlNode node)
        {
            Node = node;
            Ids = Node.ChildNodes.Where(t => t.Attributes["id"] != null).ToArray();
        }

        public HtmlNode[] Ids { set; get; }
        public bool IsScript()
        {
            if (Node.Name.ToLower().Contains("script"))
                return true;
            return false;
        }
    }

    public static class HtmlNodeHelper
    {
        public static SmartHtmlNode ToSmart(this HtmlNode node)
        {
            return new SmartHtmlNode(node);
        }
        #region Check Is<>
        public static bool IsStrong(this HtmlNode node)
        {
            if (node.Name.ToLower() == "strong")
                return true;
            return false;
        }
        public static bool IsClass(this HtmlNode node)
        {
            if (node.Attributes.Where(c => c.Value.ToLower().Contains("class")) != null)
                return true;
            return false;
        }

        public static bool IsScript(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("script"))
                return true;
            return false;
        }
        public static bool IsNoScript(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("noscript"))
                return true;
            return false;
        }
        public static bool IsLabel(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("label"))
                return true;
            return false;
        }

        public static bool IsLi(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("li"))
                return true;
            return false;
        }

        public static bool IsUl(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("ul"))
                return true;
            return false;
        }

        public static bool IsSpan(this HtmlNode node)
        {
            if (node.Name.ToLower() == "span")
                return true;
            return false;
        }

        public static bool IsStyle(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("style"))
                return true;
            return false;
        }

        public static bool IsHref(this HtmlNode node)
        {
            if (node.Name.ToLower() == "a")
                return true;
            return false;
        }
        public static bool IsHtml(this HtmlNode node)
        {
            if (node.Name.ToLower() == "html")
                return true;
            return false;
        }
        public static bool IsP(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("p"))
                return true;
            return false;
        }

        public static bool IsDiv(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("div"))
                return true;
            return false;
        }

        public static bool IsImg(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("img"))
                return true;
            return false;
        }

        public static bool IsHead(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("head"))
                return true;
            return false;
        }

        public static bool IsInput(this HtmlNode node)
        {
            if (node.Attributes.Select(t=>t.Value).Contains("head"))
                return true;
            return false;
        }

        public static bool IsTitle(this HtmlNode node)
        {
            if (node.Name.ToLower().Contains("title"))
                return true;
            return false;
        }
        #endregion


        public static int Depth(this HtmlNode node)
        {
            int _count = node.XPath.CountOf("/");
            return _count;
        }

        public static bool CanTitle(this HtmlNode node)
        {
            if (node.IsTitle())
            {
                return true;
            }
            return false;
        }

        public static bool CanContent(this HtmlNode node)
        {
            if (node.IsTitle())
            {
                return true;
            }
            return false;
        }

        
        public static bool HasMeaning(this HtmlNode node)
        {
            //if(node.IsTitle())
            //{
            //    return true;
            //}
            //return false;
            if (!node.IsScript()
                && !node.IsHtml()
                && !node.IsHead()
                && !node.IsSpan()
                && !node.IsStyle()
                && !node.IsUl()
                && !node.IsLi()
                && !node.IsNoScript()
                && !node.IsLabel()
                && !node.IsHref()
                && !node.IsP()
                && !node.IsImg()
                && !node.IsStrong())
            //&& (node.InnerText.GetCountWords() > 10))
            {
                return true;
            }
            return false;
        }
        
        public static string ToFullString(this HtmlNode node)
        {
            string _result = string.Empty;
            if (node != null)
            {
                _result = node.Depth() + ")" + node.Name + "|" + node.XPath + "<br />";

                foreach (var _att in node.Attributes)
                {
                    _result = _result + " || " + _att.Value + "<br />";
                }
                _result += _result + node.OuterHtml + "<br />";
                return _result;
            }
            return string.Empty;
        }

        public static List<HtmlNode> GetNodesByClasses(this HtmlNode node, string className)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(node.InnerHtml);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Attributes["class"] != null)
                                            && x.Attributes["class"].Value.Contains(className))
                                            .ToList();
            return _Nodes;
        }

        public static IEnumerable<HtmlNode> GetMeaningNodes(this HtmlNode node)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(node.InnerHtml);

            int _meaningDepth = doc.MeaningDepth();
            var _nodes = doc.GetNodes(_meaningDepth);
            return _nodes;
        }

        public static string[] GetLinks(this HtmlNode node)
        {
            List<string> _links = new List<string>();
            HtmlNodeCollection _nodes = node.SelectNodes("//a[@href]");
            if (_nodes != null && _nodes.Count > 0)
            {
                foreach (HtmlNode link in _nodes)
                {
                    _links.Add(link.Attributes["href"].Value);
                }
            }

            return _links.ToArray();
        }

        public static string[] GetImages(this HtmlNode node)
        {
            var _nodes = node.SelectNodes("//img");
            List<string> _imageUrls = new List<string>();
            if (_nodes != null && _nodes.Count > 0)
            {
                _imageUrls = _nodes.Where(t=>t.Attributes.Count > 0 && t.Attributes["src"] != null)
                    .Select(n => n.Attributes["src"].Value).Distinct().ToList();
            }

            List<string> _images = new List<string>();
            if (_imageUrls != null && _imageUrls.Count > 0)
            {
                foreach (var _imageUrl in _imageUrls)
                {
                    _images.Add(_imageUrl);
                }
            }

            return _images.ToArray();
        }
    }
}
