using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CafeT.Text;

namespace CafeT.Html
{

    public static class HtmlMining
    {
        //http://stackoverflow.com/questions/6383280/net-remove-javascript-and-css-code-blocks-from-html-page
        public static string RemoveScriptsAndStyles(this HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//script|//style");
            if(nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                    node.ParentNode.RemoveChild(node);
            }
            
            return doc.DocumentNode.OuterHtml; ;
        }

        public static string BoundMeaningClass(this HtmlDocument doc)
        {
            string _meaningClass = doc.MeaningClass();
            var _meaningNodes = doc.GetMeaningNodes();
            var _meaningNodesByClass = doc.GetNodesByClass(_meaningClass);

            return string.Empty;
        }

        public static string MeaningClass(this HtmlDocument doc)
        {
            int _meaningDepth = doc.MeaningDepth();
            var _nodes = doc.GetNodes(_meaningDepth);
            var _node = _nodes.Where(x => (x.Attributes["class"] != null))
                .OrderByDescending(t => t.OuterHtml.Length)
                .FirstOrDefault();
            string _name = _node.Attributes.Select(t => t.Value).FirstOrDefault();
            return _name;
        }

        public static int MeaningDepth(this HtmlDocument doc)
        {
            int _maxDepth = doc.MaxDepth();
            int i = 20;
            int _meaningDepth = doc.MeaningDepth(i);

            var _nodes = doc.GetNodes(_meaningDepth).Where(c => c.HasMeaning()).ToList();
            while ((_nodes == null) || (_nodes.Count <= 5))
            {
                i = i - 1;
                _meaningDepth = i;
                _nodes = doc.GetNodes(_meaningDepth).Where(c => c.HasMeaning()).ToList();
            }
            return _meaningDepth;
        }

        public static int MeaningDepth(this HtmlDocument doc, int? n)
        {
            int _maxDepth = doc.MaxDepth();
            int i = 0;
            while (i < _maxDepth)
            {
                List<HtmlNode> _nodes = doc.GetNodes(i).ToList();
                var _objects = _nodes.Where(c => c.HasMeaning());
                if (n.HasValue)
                {
                    if (_objects.Count() <= n.Value)
                    {
                        i = i + 1;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            return i;
        }

        public static int MaxDepth(this HtmlDocument doc)
        {
            var _nodes = doc.GetNodes();
            return _nodes.Select(c => c.Depth()).OrderByDescending(c => c).FirstOrDefault();
        }

        public static IEnumerable<HtmlNode> GetNodes(this HtmlDocument doc, int? depth)
        {
            if (depth != null && depth.HasValue)
            {
                var _nodes = doc.GetNodes().Where(c => c.Depth() == depth.Value);
                return _nodes;
            }
            return doc.GetNodes();
        }

        public static IEnumerable<HtmlNode> GetMeaningNodes(this HtmlDocument doc)
        {
            int _meaningDepth = doc.MeaningDepth();
            var _nodes = doc.GetNodes(_meaningDepth).Where(c => c.HasMeaning());
            return _nodes;
        }

        public static HtmlDocument ToHtmlDocument(this string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        public static IEnumerable<HtmlNode> GetNodesByClass(this HtmlDocument doc, string name)
        {
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

        public static IEnumerable<HtmlNode> GetNodes(this HtmlDocument doc)
        {
            List<HtmlNode> _results = new List<HtmlNode>();
            var _nodes = doc.DocumentNode.Descendants().ToList();
            if (_nodes != null && _nodes.Count > 0)
            {
                int i = 0;
                int n = _nodes.Count;
                for (i = 0; i < n; i++)
                {
                    HtmlNode _node = _nodes[i];
                    while (_node.HasChildNodes)
                    {
                        _results.Add(_node);
                        _node = _node.FirstChild;
                    }
                }
            }
            return _results;
        }


        public static HtmlNode GetNodeById(this string htmlInput, string id)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);

            List<HtmlNode> _Nodes = doc.DocumentNode.Descendants().Where(x =>
                                        (x.Name == "div"
                                            && x.Attributes["id"] != null)
                                            && x.Attributes["id"].Value.Contains(id))
                                            .ToList();
            return _Nodes.FirstOrDefault();
        }
       
        public static string HtmlToText(this string htmlString)
        {
            if (htmlString == null || htmlString.Length <= 0) return string.Empty;

            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

        public static HtmlDocument ToHtmlDocument(this List<HtmlNode> nodes)
        {
            HtmlDocument doc = new HtmlDocument();
            foreach(HtmlNode _node in nodes)
            {
                doc.DocumentNode.AppendChild(_node);
            }
            return doc;
        }

        public static string RemoveImages(this string htmlString)
        {
            string[] _images = htmlString.ExtractImages();
            if (_images.Count() >= 1)
                foreach (string _image in _images)
                {
                    htmlString = htmlString.Replace(_image, "");
                }
            return htmlString;
        }

        public static string[] ExtractImages(this string htmlString)
        {
            List<string> images = new List<string>();
            string pattern = @"<(img)\b[^>]*>";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                images.Add(matches[i].Value);
                //images.Add(matches[i].Value);
                //images.Add(matches[i].Value.Between("src=", "title", true, true));
            }

            //IF IMAGE IS EMBEDED
            //string _imgs = htmlString.ExtractFirstMinBetween("","/>")
            return images.ToArray();
        }
        
        public static async Task<string> GetHtml(this string url)
        {
            HttpClient http = new HttpClient();
            try
            {
                var response = await http.GetByteArrayAsync(url);
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                return source;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


        public static string GetHtmlEdit(this string url)
        {
            HttpClient http = new HttpClient();
            try
            {
                var response = http.GetByteArrayAsync(url).Result;
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                return source;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
