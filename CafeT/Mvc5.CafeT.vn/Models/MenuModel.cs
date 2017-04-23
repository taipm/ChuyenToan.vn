using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class MenuModel : BaseObject
    {
        public string Name { set; get; }
        public string ActionName { set; get; }
        public string ControllerName { set; get; }
        public bool Enable { set; get; }
        public int Order { set; get; }

        public IList<MenuItemModel> MenuItems { set; get; }

        public MenuModel():base()
        {
            MenuItems = new List<MenuItemModel>();
        }
    }
}