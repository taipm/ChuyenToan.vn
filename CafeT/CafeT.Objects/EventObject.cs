using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects
{
    public class EventObject : BaseObject
    {
        public List<BaseObject> Objects { set; get; }
        public EventObject()
        {
            Objects = new List<BaseObject>();
        }
    }
}
