using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Html
{
    public class HtmlManager
    {
        public string HtmlStructure(string[] htmlStrings)
        {
            if(htmlStrings.Count()<2)
            {
                return string.Empty;
            }
            else
            {
                string _firstHtml = htmlStrings[0];
                htmlStrings = htmlStrings.Where(t => t != _firstHtml).ToArray();
                foreach(string htmlString in htmlStrings)
                {
                    foreach(char c in _firstHtml)
                    {
                        if(htmlString.Contains(c))
                        {
                            _firstHtml = _firstHtml.Replace(c.ToString(),string.Empty);
                        }
                    }
                }
                return _firstHtml;
            }
        }

    }
}
