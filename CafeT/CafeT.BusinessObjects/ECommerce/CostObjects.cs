using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ECommerce
{
    public class CostObjects
    {
        public List<CostObject> Costs;
        public CostObjects()
        {
            Costs = new List<CostObject>();
        }

        public double TotalMonthly()
        {
            double _total = 0;
            foreach (var _cost in Costs)
            {
                if (_cost.Type == CostType.Daily)
                {
                    _total = _total + _cost.Value * 30;
                }
                else //Monthly
                {
                    _total = _total + _cost.Value;
                }
            }
            return _total;
        }
    }
}
