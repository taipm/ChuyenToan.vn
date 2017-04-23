using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ECommerce
{
    public class CostObject : BaseObject
    {
        public string Name { set; get; }
        public double Value { set; get; }
        public CostType Type {set;get;}
        public CostObject()
        {
            Type = CostType.Mothly;
        }
    }

    
}
