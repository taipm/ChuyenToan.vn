using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Mvc5.CafeT.vn.Helpers
{
    public static class YouTubeHelper
    {
        public static List<string> GetLinksInText(string message)
        {
            List<string> list = new List<string>();
            Regex urlRx = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

            MatchCollection matches = urlRx.Matches(message);
            foreach (Match match in matches)
            {
                list.Add(match.Value);
            }
            return list;
        }
    }
}