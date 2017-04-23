using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Enumerable
{
    //http://hintdesk.com/c-enum-of-string/
    class EnumString
    {
        public static string GetString(Enum enValue)
        {
            FieldInfo fiInfo = enValue.GetType().GetField(enValue.ToString());
            DescriptionAttribute[] daArray = fiInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return daArray.Length > 0 ? daArray[0].Description : "";
        }

        public static object GetValue(string strDescription, Type tyEnum)
        {
            string[] arrNames = Enum.GetNames(tyEnum);
            foreach (string strTemp in arrNames)
            {
                if (GetString(Enum.Parse(tyEnum, strTemp) as Enum).Equals(strDescription)) return Enum.Parse(tyEnum, strTemp);
            }
            throw new Exception("No value found for this description");
        }
    }
    //http://hintdesk.com/c-enum-of-string/
    enum BookStore
    {
        [DescriptionAttribute("www.amazon.com")]
        AMAZON,
        [DescriptionAttribute("www.barnesandnoble.com")]
        BARNESNOBLE,
        [DescriptionAttribute("www.abebooks.com")]
        ABEBOOKS,
        [DescriptionAttribute("www.borders.com")]
        BORDERS,
        [DescriptionAttribute("www.powells.com")]
        POWELLBOOKS,
        [DescriptionAttribute("www.booksamillion.com")]
        BOOKSAMILLION
    }

    public static class EnumHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }

        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

    }
}
