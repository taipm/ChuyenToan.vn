using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Html
{
    public static class HtmlStringHelper
    {
        #region License
        // Copyright (c) 2007 James Newton-King
        //
        // Permission is hereby granted, free of charge, to any person
        // obtaining a copy of this software and associated documentation
        // files (the "Software"), to deal in the Software without
        // restriction, including without limitation the rights to use,
        // copy, modify, merge, publish, distribute, sublicense, and/or sell
        // copies of the Software, and to permit persons to whom the
        // Software is furnished to do so, subject to the following
        // conditions:
        //
        // The above copyright notice and this permission notice shall be
        // included in all copies or substantial portions of the Software.
        //
        // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        // OTHER DEALINGS IN THE SOFTWARE.
        //  NOTE BY PHAN MINH TAI - 2016
        // This license for RemoveHtml
        #endregion
        public static string RemoveHtml(this string s)
        {
            return RemoveHtmlInternal(s, null);
        }

        public static string RemoveHtml(this string s, IList<string> removeTags)
        {
            if (removeTags == null)
                throw new ArgumentNullException("removeTags");

            return RemoveHtmlInternal(s, removeTags);
        }

        public static List<HtmlNode> GetAllNodes(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().ToList();
            return _Nodes;
        }

        public static List<string> GetAllNodeNames(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().ToList();
            return _Nodes.Select(c => c.Name).Distinct().ToList();
        }

        public static List<string> GetMeaningNodeNames(string html)
        {
            string[] _exclude = new string[] {"comment","p","text","input","label", "form","script","h1","h2","h3",
                "h4","ul","li","noscript","img","select","header","footer","style","html" };
            List<string> _names = GetAllNodeNames(html);
            List<string> _results = new List<string>();
            if (_names != null && _names.Count > 0)
            {
                foreach (string _name in _names)
                {
                    if (!_exclude.Contains(_name))
                    {
                        _results.Add(_name);
                    }
                }
            }
            return _results;
        }

        public static List<HtmlNode> GetAllSingleNodes(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Name == "div"
                                            && x.Attributes["class"] != null))
                                            .ToList();
            return _Nodes.Where(c => c.HasChildNodes == false).ToList();
        }

        public static IEnumerable<HtmlNode> GetNodeLinks(this string htmlInput)
        {
            List<string> _links = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            var _linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            return _linkNodes;

        }
        private static string RemoveHtmlInternal(string s, IList<string> removeTags)
        {
            List<string> removeTagsUpper = null;

            if (removeTags != null)
            {
                removeTagsUpper = new List<string>(removeTags.Count);

                foreach (string tag in removeTags)
                {
                    removeTagsUpper.Add(tag.ToUpperInvariant());
                }
            }

            Regex anyTag = new Regex(@"<[/]{0,1}\s*(?<tag>\w*)\s*(?<attr>.*?=['""].*?[""'])*?\s*[/]{0,1}>", RegexOptions.Compiled);

            return anyTag.Replace(s, delegate (Match match)
            {
                string tag = match.Groups["tag"].Value.ToUpperInvariant();

                if (removeTagsUpper == null)
                    return string.Empty;
                else if (removeTagsUpper.Contains(tag))
                    return string.Empty;
                else
                    return match.Value;
            });
        }
        public static bool IsHtmlTag(this string text)
        {
            string pattern = @"(?<=</?)([^ >/]+)";
            var matches = Regex.Matches(text, pattern);
            if(matches != null && matches.Count>0)
            {
                return true;
            }
            return false;
        }

        public static string[] GetAllHtmlTags(this string htmlString)
        {
            var _tags = new List<string>();
            string pattern = @"(?<=</?)([^ >/]+)";
            var matches = Regex.Matches(htmlString, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                _tags.Add(matches[i].ToString());
            }
            //to obtain non duplicate list
            _tags = _tags.Distinct().ToList();

            return _tags.ToArray();
        }

        public static IEnumerable<HtmlNode> GetNodesByClass(this string htmlString, string name)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Attributes["class"] != null)
                                            && x.Attributes["class"].Value.Contains(name))
                                            .ToList();
            if (_Nodes == null || _Nodes.Count == 0)
            {
                _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Name == name))
                                            .ToList();
            }
            return _Nodes;
        }
        public static IEnumerable<HtmlNode> GetNodesById(this string htmlString, string name)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Attributes["id"] != null)
                                            && x.Attributes["id"].Value.Contains(name))
                                            .ToList();
            if (_Nodes == null || _Nodes.Count == 0)
            {
                _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Name == name))
                                            .ToList();
            }
            return _Nodes;
        }
        public static string RemoveScriptsAndStyles(this string htmlString)
        {
            string Pat = "<(script|style)\\b[^>]*?>.*?</\\1>";
            return Regex.Replace(htmlString, Pat, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        #region Basics
        public static string[] GetLinks(this string htmlInput)
        {
            List<string> _links = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            var _linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (_linkNodes != null && _linkNodes.Count() > 0)
            {
                foreach (HtmlNode _link in _linkNodes)
                {
                    if (_link.Attributes["href"].Value != null)
                    {
                        _links.Add(_link.Attributes["href"].Value);
                    }
                }
            }

            return _links.Distinct().ToArray();
        }

        #endregion
        

        public static string ToHtmlTable<T>(this IEnumerable<T> list, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {

            var result = new StringBuilder();
            if (String.IsNullOrEmpty(tableSyle))
            {
                result.Append("<table id=\"" + typeof(T).Name + "Table\">");
            }
            else
            {
                result.Append("<table id=\"" + typeof(T).Name + "Table\" class=\"" + tableSyle + "\">");
            }

            var propertyArray = typeof(T).GetProperties();
            foreach (var prop in propertyArray)
            {
                if (String.IsNullOrEmpty(headerStyle))
                {
                    result.AppendFormat("<th>{0}</th>", prop.Name);
                }
                else
                {
                    result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, prop.Name);
                }
            }

            for (int i = 0; i < list.Count(); i++)
            {
                if (!String.IsNullOrEmpty(rowStyle) && !String.IsNullOrEmpty(alternateRowStyle))
                {
                    result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                }
                else
                {
                    result.AppendFormat("<tr>");
                }

                foreach (var prop in propertyArray)
                {
                    object value = prop.GetValue(list.ElementAt(i), null);
                    result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
                }
                result.AppendLine("</tr>");
            }
            result.Append("</table>");
            return result.ToString();
        }
    }
}
