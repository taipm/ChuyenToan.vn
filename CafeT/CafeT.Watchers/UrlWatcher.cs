using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CafeT.Watchers
{
    
    public class UrlWatcher:BaseObject
    {
        public string Url { set; get; }
        public Timer AutoTime { set; get; }
        public double Interval { set; get; }
        public string LastHtmlContent { set; get; }
        public string CurrentHtmlContent { set; get; }
        public string HtmlNews { set; get; }
        public DateTime LastRead { set; get; }
        public WebPage Page { set; get; }
        public int CountOfRead { set; get; }

        public UrlWatcher()
        {
        }

        public UrlWatcher(string url, double interval)
        {
            Url = url;
            Interval = interval;
            AutoTime = new Timer(Interval);
            AutoTime.Elapsed += AutoTime_Elapsed;
            AutoTime.Enabled = true; // Enable it
            CountOfRead = 0;
        }

        private void AutoTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            Read();
            //if (HasNews())
            //{
            //    GetNews();
            //    this.SaveToText(@"C:\Users\taipm\Downloads\UrlWatcher-"+ LastRead.ToShortTimeString().Replace(":","") + ".txt");
            //}
        }
        public void SaveToText(string pathFile)
        {
            StreamWriter _w = new StreamWriter(pathFile, true, UTF8Encoding.UTF8);
            string _content = this.ToString();
            _w.Write(_content);
            _w.Close();
        }
        public bool CanRead()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(Url))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public void Read()
        {
            if(CanRead())
            {
                Page = new WebPage(Url);
                if(CountOfRead == 0)
                {
                    LastRead = DateTime.Now;
                    LastHtmlContent = Page.HtmlSmart.HtmlContent;
                    CurrentHtmlContent = LastHtmlContent;
                }
                else
                {
                    LastRead = DateTime.Now;
                    LastHtmlContent = CurrentHtmlContent;
                    CurrentHtmlContent = Page.HtmlSmart.HtmlContent;
                }
                CountOfRead = CountOfRead + 1;
            }
        }

        //public bool HasNews()
        //{
        //    return LastHtmlContent.IsDifference(CurrentHtmlContent);
        //}

        public string GetNews()
        {
            HtmlNews =  "+ HasNews - Not implementation";
            return HtmlNews;
        }

        public override string ToString()
        {
            return this.GetObjectInfo("Url, LastRead, HtmlNews");
        }
    }
}
