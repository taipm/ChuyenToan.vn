using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class YouTubeView
    {
        public string EmbedUrl { set; get; }
        public YouTubeView(string watchUrl)
        {
            string _before = @"<div class=""video-container""> <iframe width=""854"" height=""480"" src=";
            string _end = " frameborder=\"0\" allowfullscreen></iframe></div>";
            string _youtubeLink = string.Empty;
            _youtubeLink = watchUrl.AddBefore(_before).AddAfter(_end).Replace("watch?v=", "embed/");
            EmbedUrl = _youtubeLink;
        }
    }
}