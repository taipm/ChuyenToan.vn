using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class MenuItemModel:BaseObject
    {
        public string Name { set; get; }
        public string ActionName { set; get; }
        public string ControllerName { set; get; }
        public string Url { set; get; }
        public MenuModel ParentMenu { set; get; }

        public bool Enable { set; get; }
        public int Order { set; get; }

        public MenuItemModel():base()
        {

        }
    }
}