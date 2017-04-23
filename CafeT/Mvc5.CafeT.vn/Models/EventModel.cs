using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class EventModel:EventObject
    {
        public IEnumerable<FileModel> Files { set; get; }
        public IEnumerable<ImageModel> Images { set; get; }

        public EventModel():base()
        {
        }
    }
}