using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartCrawler.Models
{
    public class CrawlerFilters
    {
        public string Name { set; get; }
        public string[] Skips { set; get; }
        public CrawlerFilters()
        {
            Skips = new string[] { };
        }
    }
}
