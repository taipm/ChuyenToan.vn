using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public class ObjectWrapper : IFormattable
    {
        private readonly object wrapped;
        private static readonly Dictionary<string, FormatInfo> Cache = new Dictionary<string, FormatInfo>();

        public ObjectWrapper(object wrapped)
        {
            this.wrapped = wrapped;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                return this.wrapped.ToString();
            }

            var type = this.wrapped.GetType();
            var key = type.FullName + ":" + format;

            FormatInfo wrapperCache;
            lock (Cache)
            {
                if (!Cache.TryGetValue(key, out wrapperCache))
                {
                    wrapperCache = CreateFormatInfo(format, type);
                    Cache.Add(key, wrapperCache);
                }
            }

            var propertyInfo = wrapperCache.PropertyInfo;
            var outputFormat = wrapperCache.OutputFormat;

            var value = propertyInfo != null ? propertyInfo.GetValue(this.wrapped) : this.wrapped;

            return string.Format(formatProvider, outputFormat, value);
        }

        private static FormatInfo CreateFormatInfo(string format, IReflect type)
        {
            var spilt = format.Split(new[] { ':' }, 2);
            var param = spilt[0];
            var hasSubFormat = spilt.Length == 2;
            var subFormat = hasSubFormat ? spilt[1] : string.Empty;

            var propertyInfo = type.GetProperty(param, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            var outputFormat = propertyInfo != null ? (hasSubFormat ? "{0:" + subFormat + "}" : "{0}") : "{0:" + format + "}";

            return new FormatInfo(propertyInfo, outputFormat);
        }

        private class FormatInfo
        {
            public FormatInfo(PropertyInfo propertyInfo, string form)
            {
                this.PropertyInfo = propertyInfo;
                this.OutputFormat = form;
            }

            public PropertyInfo PropertyInfo { get; private set; }

            public string OutputFormat { get; private set; }
        }
    }
}
