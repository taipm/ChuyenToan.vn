using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CafeT.SmartCrawler
{
    public static class GenericUtils
    {
        public static T Load<T>(string FileName)
        {
            Object rslt;
            if (File.Exists(FileName))
            {
                var xs = new XmlSerializer(typeof(T));
                using (var sr = new StreamReader(FileName))
                {
                    rslt = (T)xs.Deserialize(sr);
                }
                return (T)rslt;
            }
            else
            {
                return default(T);
            }
        }

        public static bool Save<T>(string FileName, Object Obj)
        {
            var xs = new XmlSerializer(typeof(T));
            using (TextWriter sw = new StreamWriter(FileName))
            {
                xs.Serialize(sw, Obj);
            }
            if (File.Exists(FileName))
                return true;
            else return false;
        }

        public static bool Save2(string FileName, Object Obj)
        {
            var xs = new XmlSerializer(Obj.GetType());
            using (TextWriter sw = new StreamWriter(FileName))
            {
                xs.Serialize(sw, Obj);
            }
            if (File.Exists(FileName))
                return true;
            else return false;
        }

        public static Object Load2(string FileName)
        {
            Object rslt;
            if (File.Exists(FileName))
            {
                var xs = new XmlSerializer(typeof(Object));
                using (var sr = new StreamReader(FileName))
                {
                    rslt = (Object)xs.Deserialize(sr);
                }
                return (Object)rslt;
            }
            else
            {
                return default(Object);
            }
        }


    }
}
