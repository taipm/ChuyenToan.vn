using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Enumerable
{
    //http://www.extensionmethod.net/csharp/ienumerable-t/tocsv

    public static class EnumerableHelper
    {
        /// <summary>
        /// Write all elements in the Enumeration to the Console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void WriteToConsole<T>(this IEnumerable<T> list)
        {
            foreach (var obj in list)
            {
                Console.WriteLine(obj);
            }
        }

        /// <summary>
        /// Write all elements in the Enumeration to the Console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">the enumeration written to the list</param>
        /// <param name="predicate">a transform function to apply to each element</param>
        public static void WriteToConsole<T>(this IEnumerable<T> list, Func<T, object> transfer)
        {
            foreach (var obj in list)
            {
                Console.WriteLine(transfer(obj));
            }
        }

        public static bool IsSorted<T>(this IEnumerable<T> @this, Comparison<T> comparison = null)
        {
            if (comparison == null)
                comparison = Comparer<T>.Default.Compare;

            using (IEnumerator<T> e = @this.GetEnumerator())
            {
                if (!e.MoveNext())
                    return true;

                T prev = e.Current;
                while (e.MoveNext())
                {
                    T current = e.Current;
                    if (comparison(prev, current) > 0)
                        return false;

                    prev = current;
                }
            }
            return true;
        }
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, bool descending)
        {
            if (enumerable == null)
            {
                return null;
            }

            if (descending)
            {
                return enumerable.OrderByDescending(keySelector);
            }

            return enumerable.OrderBy(keySelector);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, IComparable> keySelector1, Func<TSource, IComparable> keySelector2, params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return null;
            }

            IEnumerable<TSource> current = enumerable;

            if (keySelectors != null)
            {
                for (int i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i]);
                }
            }

            current = current.OrderBy(keySelector2);

            return current.OrderBy(keySelector1);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> enumerable, bool descending, Func<TSource, IComparable> keySelector, params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return null;
            }

            IEnumerable<TSource> current = enumerable;

            if (keySelectors != null)
            {
                for (int i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i], descending);
                }
            }

            return current.OrderBy(keySelector, descending);
        }

        public static string ToHtmlTable<T>(this IEnumerable<T> list, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {

            var result = new StringBuilder();
            if (String.IsNullOrEmpty(tableSyle))
            {
                result.Append("<table id=\"" + typeof(T).Name + "Table\">");
            }
            else
            {
                result.Append("<table id=\"" + typeof(T).Name + "Table\" class=\"" + tableSyle + "\">");
            }

            var propertyArray = typeof(T).GetProperties();
            foreach (var prop in propertyArray)
            {
                if (String.IsNullOrEmpty(headerStyle))
                {
                    result.AppendFormat("<th>{0}</th>", prop.Name);
                }
                else
                {
                    result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, prop.Name);
                }
            }

            for (int i = 0; i < list.Count(); i++)
            {
                if (!String.IsNullOrEmpty(rowStyle) && !String.IsNullOrEmpty(alternateRowStyle))
                {
                    result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                }
                else
                {
                    result.AppendFormat("<tr>");
                }

                foreach (var prop in propertyArray)
                {
                    object value = prop.GetValue(list.ElementAt(i), null);
                    result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
                }
                result.AppendLine("</tr>");
            }
            result.Append("</table>");
            return result.ToString();
        }

        public static bool None<T>(this IEnumerable<T> source)
        {
            return source.Any() == false;
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> query)
        {
            return source.Any(query) == false;
        }

        public static bool Many<T>(this IEnumerable<T> source)
        {
            return source.Count() > 1;
        }

        public static bool Many<T>(this IEnumerable<T> source, Func<T, bool> query)
        {
            return source.Count(query) > 1;
        }

        public static bool OneOf<T>(this IEnumerable<T> source)
        {
            return source.Count() == 1;
        }

        public static bool OneOf<T>(this IEnumerable<T> source, Func<T, bool> query)
        {
            return source.Count(query) == 1;
        }

        public static bool XOf<T>(this IEnumerable<T> source, int count)
        {
            return source.Count() == count;
        }

        public static bool XOf<T>(this IEnumerable<T> source, Func<T, bool> query, int count)
        {
            return source.Count(query) == count;
        }

        /// <summary>
        /// Continues processing items in a collection until the end condition is true.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection to iterate.</param>
        /// <param name="endCondition">The condition that returns true if iteration should stop.</param>
        /// <returns>Iterator of sub-list.</returns>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> collection, Predicate<T> endCondition)
        {
            return collection.TakeWhile(item => !endCondition(item));
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> collection, int start, int end)
        {
            int index = 0;
            int count = 0;

            if (collection == null)
                throw new ArgumentNullException("collection");

            // Optimise item count for ICollection interfaces.
            if (collection is ICollection<T>)
                count = ((ICollection<T>)collection).Count;
            else if (collection is ICollection)
                count = ((ICollection)collection).Count;
            else
                count = collection.Count();

            // Get start/end indexes, negative numbers start at the end of the collection.
            if (start < 0)
                start += count;

            if (end < 0)
                end += count;

            foreach (var item in collection)
            {
                if (index >= end)
                    yield break;

                if (index >= start)
                    yield return item;

                ++index;
            }
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return retVal;
        }
        public static int FirstIndex<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static int LastIndex<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (var item in list.Reverse())
            {
                if (predicate(item))
                {
                    return list.Count() - index - 1;
                }
                index++;
            }
            return -1;
        }
    }
}
