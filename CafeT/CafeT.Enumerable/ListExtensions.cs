using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Enumerable
{
    //http://hintdesk.com/c-sort-list-of-filename-according-to-its-extension/
    class ExtensionSorter : IComparer<string>
    {
        #region IComparer<string> Members

        public int Compare(string x, string y)
        {
            string strExt1 = Path.GetExtension(x);
            string strExt2 = Path.GetExtension(y);

            if (strExt1.Equals(strExt2))
            {
                return x.CompareTo(y);
            }
            else
            {
                return strExt1.CompareTo(strExt2);
            }
        }

        #endregion
    }

    public static class ListExtensions
    {

        public static TList With<TList, T>(this TList list, T item) where TList : IList<T>, new()
        {
            TList l = new TList();

            foreach (T i in list)
            {
                l.Add(i);
            }
            l.Add(item);

            return l;
        }

        public static TList Without<TList, T>(this TList list, T item) where TList : IList<T>, new()
        {
            TList l = new TList();

            foreach (T i in list.Where(n => !n.Equals(item)))
            {
                l.Add(i);
            }

            return l;
        }
    }
}
