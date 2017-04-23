using CafeT.SmartFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CafeT.Html
{
    public class WebPageCssConfig
    {
        /// <summary>
        /// Notes
        /// - The configFile must to save as text file
        /// - Url | .................
        /// - UrlCss|Value
        /// - TitleCss | .............
        /// </summary>
        public Guid WebPageId { set; get; }
        public string PathConfig { set; get; }
        public string UrlCss { set; get; }
        public string TitleCss { set; get; }
        public string MetaCss { set; get; }
        public string DescriptionCss { set; get; }
        public string ContentCss { set; get; }
        public string TagsCss { set; get; }

        public string ErrorMsg { set; get; }

        public WebPageCssConfig()
        {
        }

        public WebPageCssConfig(string path)
        {
            if (File.Exists(path))
            {
                SmartTextFile _smartFile = new SmartTextFile(path);
                UrlCss = _smartFile.GetFirstLine("UrlCss");
                TitleCss = _smartFile.GetFirstLine("TitleCss");
                MetaCss = _smartFile.GetFirstLine("MetaCss");
                DescriptionCss = _smartFile.GetFirstLine("DescriptionCss");
                ContentCss = _smartFile.GetFirstLine("ContentCss");
                TagsCss = _smartFile.GetFirstLine("TagsCss");
            }
            else
            {
                ErrorMsg = "The file in " + path + " does not exits";
            }
        }
    }
}