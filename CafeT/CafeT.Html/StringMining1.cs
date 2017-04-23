using CafeT.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.DataMining
{
    public static class StringMining
    {
        public static string[] GetLatex(this string text)
        {
            string[] strSplits = new string[] { "$$" };
            return text.Split(strSplits, StringSplitOptions.None);
        }

        public static bool ContainsLatex(this string text)
        {
            return text.Contains("$$");
        }

        public static bool ContainsHtmlTag(this string text, string tag)
        {
            var pattern = @"<\s*" + tag + @"\s*\/?>";
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        public static bool ContainsHtmlTags(this string text, string tags)
        {
            var ba = tags.Split('|').Select(x => new { tag = x, hastag = text.ContainsHtmlTag(x) }).Where(x => x.hastag);

            return ba.Count() > 0;
        }

        public static bool ContainsHtmlTags(this string text)
        {
            return text.ContainsHtmlTags(
                    "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var");
        }

        public static bool IsHtml(this string input)
        {
            if (input.ContainsHtmlTags())
                return true;
            return false;
        }

       
        public static string[] ToTags(this string input, char split)
        {
            input = input.ToStandard();
            char[] splitChars = { split };
            string[] _tags = input.Split(splitChars);
            return _tags;
        }
    }
}
