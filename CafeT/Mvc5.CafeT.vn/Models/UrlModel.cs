using CafeT.BusinessObjects;
using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class UrlModel:BaseObject
    {
        public string Url { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        //public bool IsToWebPage { set; get; }

        public UrlModel():base()
        {
            Url = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }

        public UrlModel(string url)
        {
            Url = url;
        }             
    }
}