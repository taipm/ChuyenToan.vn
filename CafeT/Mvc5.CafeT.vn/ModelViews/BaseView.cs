using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class Error
    {
        public Dictionary<int, string> dictErrors { set; get; }
        public int Id;
        public string Message;
        public Error()
        {
            dictErrors.Add(1,"Wrong username/password");
            dictErrors.Add(2, "Field must than 2 words");
        }
    }

    public class BaseView
    {
        [Key]
        public Guid Id { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? LastUpdatedDate { set; get; }
        public string LastUpdatedBy { set; get; }

        public string PageTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
    }
}