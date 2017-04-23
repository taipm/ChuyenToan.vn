using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ApplicationMessage:BaseObject
    {
        public string Name { set; get; }

        public string Message { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDisplay { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDisplay { set; get; }

        public ApplicationMessage():base()
        {
            StartDisplay = DateTime.Now;
            EndDisplay = StartDisplay.AddDays(3);
        }
        public bool IsDisplay()
        {
            if (EndDisplay >= DateTime.Now) return true;
            return false;
        }
    }
}