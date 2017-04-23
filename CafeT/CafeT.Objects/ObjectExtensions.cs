using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CafeT.Objects
{
    public static class ObjectExtensions
    {
        public static Exception ToException(this object o)
        {
            return new Exception(o.ToString());
        }

        public static object Update<T>(this T instance, string propName, object value) where T : class
        {
            Type myType = typeof(T);
            FieldInfo myFieldInfo = myType.GetField(propName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Change the field value using the SetValue method. 
            myFieldInfo.SetValue(instance, value);
            return instance;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        
        public static Dictionary<string, object> Fields<T>(this T instance) where T : class
        {
            Dictionary<string, object> _dict = new Dictionary<string, object>();
            // Include the type of the object
            System.Type _type = instance.GetType();
            FieldInfo[] fi = _type.GetFields();
            if (fi.Length > 0)
            {
                foreach (FieldInfo f in fi)
                {
                    _dict.Add(f.ToString(), f.GetValue(instance));
                }
            }

           
            PropertyInfo[] pi = _type.GetProperties();
            if (pi.Length > 0)
            {
                foreach (PropertyInfo p in pi)
                {
                    _dict.Add(p.ToString(), p.GetValue(instance));
                }
            }

            return _dict;
        }

        public static string GetObjectAllFields<T>(this T instance) where T : class
        {
            StringBuilder sb = new StringBuilder();

            // Include the type of the object
            System.Type type = instance.GetType();
            sb.Append("Type: " + type.Name);

            // Include information for each Field
            sb.Append("\r\n\r\nFields:");
            System.Reflection.FieldInfo[] fi = type.GetFields();
            if (fi.Length > 0)
            {
                foreach (FieldInfo f in fi)
                {
                    sb.Append("\r\n " + f.ToString() + " = " + f.GetValue(instance));
                }
            }
            else
                sb.Append("\r\n None");

            // Include information for each Property
            sb.Append("\r\n\r\nProperties:");
            System.Reflection.PropertyInfo[] pi = type.GetProperties();
            if (pi.Length > 0)
            {
                foreach (PropertyInfo p in pi)
                {
                    sb.Append("\r\n " + p.ToString() + " = " +
                              p.GetValue(instance, null));
                }
            }
            else
            {
                sb.Append("\r\n None");
            }

            return sb.ToString();
        }
        //http://www.codeguru.com/csharp/csharp/cs_syntax/reflection/article.php/c14531/NET-Tip-Display-All-Fields-and-Properties-of-an-Object.htm
        public static string GetObjectInfo<T>(this T instance, string fields) where T : class
        {
            StringBuilder sb = new StringBuilder();

            // Include the type of the object
            System.Type type = instance.GetType();
            sb.Append("Type: " + type.Name);

            // Include information for each Field
            sb.Append("\r\n\r\nFields:");
            System.Reflection.FieldInfo[] fi = type.GetFields();
            if (fi.Length > 0)
            {
                foreach (FieldInfo f in fi)
                {
                    sb.Append("\r\n " + f.ToString() + " = " + f.GetValue(instance));
                }
            }
            else
                sb.Append("\r\n None");

            // Include information for each Property
            sb.Append("\r\n\r\nProperties:");
            System.Reflection.PropertyInfo[] pi = type.GetProperties();
            if (pi.Length > 0)
            {
                foreach (PropertyInfo p in pi)
                {
                    if(fields.Contains(p.Name))
                    {
                        sb.Append("\r\n " + p.ToString() + " = " +
                              p.GetValue(instance, null));
                    }
                }
            }
            else
            {
                sb.Append("\r\n None");
            }
                
            return sb.ToString();
        }

        public static string PrintAllProperties<T>(this T instance) where T : class
        {

            if (instance == null)
                return string.Empty;

            var strListType = typeof(List<string>);
            var strArrType = typeof(string[]);

            var arrayTypes = new[] { strListType, strArrType };
            var handledTypes = new[] { typeof(bool), typeof(Int32), typeof(String), typeof(DateTime), typeof(double), typeof(decimal), strListType, strArrType };

            var validProperties = instance.GetType()
                                          .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                          .Where(prop => handledTypes.Contains(prop.PropertyType))
                                          .Where(prop => prop.GetValue(instance, null) != null)
                                          .ToList();

            var format = string.Format("{{0,-{0}}} : {{1}}", validProperties.Max(prp => prp.Name.Length));

            return string.Join(
                     Environment.NewLine,
                     validProperties.Select(prop => string.Format(format,
                                                                  prop.Name,
                                                                  (arrayTypes.Contains(prop.PropertyType) 
                                                                  ? string.Join(", ", (IEnumerable<string>)prop.GetValue(instance, null))                                        
                                                                  : prop.GetValue(instance, null)))));
        }

        public static void Print(this ExpandoObject dynamicObject)
        {
            var dynamicDictionary = dynamicObject as IDictionary<string, object>;

            foreach (KeyValuePair<string, object> property in dynamicDictionary)
            {
                Console.WriteLine("{0}: {1}", property.Key, property.Value.ToString());
            }
            Console.WriteLine();
        }

        public static Dictionary<string, object> GetPropertyDictionary(this object source)
        {
            var properties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var result = properties.ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(source));

            return result;
        }

        
        public static string ToDelimitedString<T>(this T obj, char delimiter, Func<T, System.Reflection.PropertyInfo, string> func)
        {
            if (obj == null || func == null)
                return null;

            var builder = new StringBuilder();
            var props = obj.GetType().GetProperties();
            for (int p = 0; p < props.Length; p++)
                builder.Append(func(obj, props[p]) + delimiter.ToString());

            //Remove the last character, the last delimiter
            if (builder.Length > 0)
                return builder.ToString().Remove(builder.ToString().Length - 1);

            return null;
        }
        public static string ToCommaDelimitedString(this object obj)
        {
            return obj.ToDelimitedString<object>(',',
                (object o, System.Reflection.PropertyInfo p) => { return (string.Format("{0}.{1}={2}", p.ReflectedType.Name, p.Name, Convert.ToString(p.GetValue(o, null)))); });
        }

        public static string ToPipeDelimitedString(this object obj)
        {
            return obj.ToDelimitedString<object>('|',
                (object o, System.Reflection.PropertyInfo p) => { return (string.Format("{0}.{1}={2}", p.ReflectedType.Name, p.Name, Convert.ToString(p.GetValue(o, null)))); });
        }
        public static void IfType<T>(this object item, Action<T> action) where T : class
        {
            if (item is T)
            {
                action(item as T);
            }
        }
        public static string ToSortedString(this object value)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            SortedDictionary<string, string> values = new SortedDictionary<string, string>();

            PropertyInfo[] properties = value.GetType().GetProperties(bindingFlags);
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(value, null);
                string maskedValue = propertyValue == null ? "null" : propertyValue.ToString();

                values.Add(propertyName, maskedValue);
            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> item in values)
            {
                sb.AppendFormat("{0}={1}{2}", item.Key, item.Value, Environment.NewLine);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Convert the object properties to a dictionary
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        /// <summary>
        /// Converts the object properties to a dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);

            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
            {
                dictionary.Add(property.Name, (T)value);
            }
            else
            {
                T newValue = (T)Convert.ChangeType(value, typeof(T));
                dictionary.Add(property.Name, newValue);
            }
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }

        public static T FindMin<T, TValue>(this IEnumerable<T> list, Func<T, TValue> predicate)
                                                        where TValue : IComparable<TValue>
        {

            T result = list.FirstOrDefault();
            if (result != null)
            {
                var bestMin = predicate(result);
                foreach (var item in list.Skip(1))
                {
                    var v = predicate(item);
                    if (v.CompareTo(bestMin) < 0)
                    {
                        bestMin = v;
                        result = item;
                    }
                }
            }
            return result;
        }

        public static T FindMax<T, TValue>(this IEnumerable<T> list, Func<T, TValue> predicate)
                                                                where TValue : IComparable<TValue>
        {
            T result = list.FirstOrDefault();
            if (result != null)
            {
                var bestMax = predicate(result);
                foreach (var item in list.Skip(1))
                {
                    var v = predicate(item);
                    if (v.CompareTo(bestMax) > 0)
                    {
                        bestMax = v;
                        result = item;
                    }
                }
            }
            return result;
        }
    }
}
