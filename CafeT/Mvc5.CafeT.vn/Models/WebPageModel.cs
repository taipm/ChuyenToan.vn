using CafeT.BusinessObjects;
using CafeT.Html;
using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class WebPageModel : WebPageObject
    {
        HtmlParseError Errors { set; get; }
        public WebPageModel():base()
        {
        }
        public WebPageModel(string url):base(url)
        {
            Remove();
        }
        public void Remove()
        {
            try
            {
                this.Page.HtmlSmart.Document.RemoveScriptsAndStyles();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //this.Page.HtmlSmart.Document.RemoveScriptsAndStyles();
        }
    }
}