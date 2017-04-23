using CafeT.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.SmartObjects
{
    public class SmartUrl:BaseObject
    {
        public string Url { set; get; }

        public SmartUrl(string url)
        {
            Url = url;
        }

        public bool IsLive()
        {
            if (CanRead())
                return true;
            else
                return false;
        }

        public bool CanRead()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    using (var stream = client.OpenRead(Url))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
