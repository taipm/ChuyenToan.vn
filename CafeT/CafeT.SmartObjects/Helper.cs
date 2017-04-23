using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartObjects
{
    public static class Helper
    {
        public static SmartText ToSmartText(this string text)
        {
            return new SmartText(text);
        }
    }
}
