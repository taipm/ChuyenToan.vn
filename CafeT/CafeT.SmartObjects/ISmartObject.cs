using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartObjects
{
    interface ISmartObject
    {
        void SelectMethod();
        void Excute(string method);
    }
}
