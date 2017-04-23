using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CafeT.Html
{
    /// <summary>
    /// Nhận thông tin đầu vào là chuỗi Html - gọi là HtmlString
    /// Không nhận bất cứ định dạng nào khác, do việc đó đã có các đối tượng khác lo
    /// </summary>
    public class SmartHtml:HtmlDocument
    {
        public string HtmlContent { set; get; }
        public string TextContent { set; get; }
        public string[] Images { set; get; }
        public string[] InternalLinks { set; get; }
        public string[] ExternalLinks { set; get; }

        public HtmlDocument Document { set; get; }
        public IEnumerable<HtmlNode> Nodes { set; get; }
        public IEnumerable<HtmlNode> MeaningNodes { set; get; }
        public IEnumerable<string> CssClasses { set; get; }

        public SmartHtml(string htmlString)
        {
            if(!string.IsNullOrEmpty(htmlString))
            {
                HtmlContent = htmlString;
                Document = new HtmlDocument();
                Document.LoadHtml(HtmlContent);
                TextContent = Document.DocumentNode.OuterHtml.HtmlToText();
                InternalLinks = Document.DocumentNode.GetLinks();
                Images = Document.DocumentNode.GetImages();
                Nodes = Document.GetNodes();
                if (Nodes != null && Nodes.Count() > 0)
                {
                    MeaningNodes = Nodes
                        .Where(t => t.HasMeaning()).Distinct();
                }
            }
            
            //CssClasses = Document.GetClasses();
        }
        
        public void SaveAsHtml(string fileName, Encoding endcoding)
        {
            Document.Save(fileName, endcoding);
        }
    }
}
