using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class WorkModel:BaseObject
    {
        public Guid? UserId { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string[] Tags { set; get; }
        public string Location { set; get; }
        public Address Address { set; get; }
        public double SalaryOnHour { set; get; }
        public WorkModel() : base()
        {
        }
    }

    public class WorkCategory:BaseObject
    {
        public string Name { set; get; }
    }
}