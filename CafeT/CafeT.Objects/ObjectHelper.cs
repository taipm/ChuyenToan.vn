using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
    public static class ObjectHelper
    {
        public static string ToXml<T>(this T o) where T : new()
        {
            string retVal;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(T));
                xs.Serialize(ms, o);
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                retVal = sr.ReadToEnd();
            }
            return retVal;
        }

        public static string SerializeToXml(this object obj)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xmlWriter = doc.CreateWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(xmlWriter, obj);
                xmlWriter.Close();
            }
            return doc.ToString();
        }
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJson(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }

        public static T FromJson<T>(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(obj as string);
        }

        public static object CloneObject(this object objSource)
        {
            //Get the type of source object and create a new instance of that type

            Type typeSource = objSource.GetType();

            object objTarget = Activator.CreateInstance(typeSource);

            //Get all the properties of source object type

            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);



            //Assign all source property to taget object 's properties

            foreach (PropertyInfo property in propertyInfo)

            {

                //Check whether property can be written to

                if (property.CanWrite)

                {

                    //check whether property type is value type, enum or string type

                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))

                    {

                        property.SetValue(objTarget, property.GetValue(objSource, null), null);

                    }

                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached

                    else

                    {

                        object objPropertyValue = property.GetValue(objSource, null);

                        if (objPropertyValue == null)

                        {

                            property.SetValue(objTarget, null, null);

                        }

                        else

                        {

                            property.SetValue(objTarget, objPropertyValue.CloneObject(), null);

                        }

                    }

                }

            }

            return objTarget;
        }
        /// <summary>
        /// Makes a copy from the object.
        /// Doesn't copy the reference memory, only data.
        /// </summary>
        /// <typeparam name="T">Type of the return object.</typeparam>
        /// <param name="item">Object to be copied.</param>
        /// <returns>Returns the copied object.</returns>
        public static T Clone<T>(this object item)
        {
            if (item != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();

                formatter.Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);

                T result = (T)formatter.Deserialize(stream);

                stream.Close();

                return result;
            }
            else
                return default(T);
        }

        //https://stephenhaunts.com/2014/11/28/serializing-pocos-to-byte-arrays/

        public static byte[] Serialize(this Object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, obj);

                var compressed = Compress(memoryStream.ToArray());
                return compressed;
            }
        }

        public static Object DeSerialize(this byte[] arrBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                var decompressed = Decompress(arrBytes);

                memoryStream.Write(decompressed, 0, decompressed.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        private static byte[] Compress(byte[] input)
        {
            byte[] compressesData;

            using (var outputStream = new MemoryStream())
            {
                using (var zip = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zip.Write(input, 0, input.Length);
                }

                compressesData = outputStream.ToArray();
            }

            return compressesData;
        }

        private static byte[] Decompress(byte[] input)
        {
            byte[] decompressedData;

            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(input))
                {
                    using (var zip = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outputStream);
                    }
                }

                decompressedData = outputStream.ToArray();
            }

            return decompressedData;
        }


        public static bool IsNullType(this object obj)
        {
            return (obj == null || obj is DBNull);
        }

        public static bool IsSigned(this object obj)
        {
            return (obj is ValueType && (obj is Int32 || obj is Int64 || obj is Int16 || obj is IntPtr || obj is decimal || obj is SByte));
        }

        public static bool IsEmpty(this object obj)
        {
            return (!IsNullType(obj) && (
               (obj is String && ((string)obj).Length == 0) ||
               (obj is StringBuilder && ((StringBuilder)obj).Length == 0) ||
               (obj is ICollection && ((ICollection)obj).Count == 0) ||
               (obj is Array && ((Array)obj).Length == 0) ||
               (IsSigned(obj) && obj == (ValueType)(-1)) ||
               (obj is ValueType && obj == (ValueType)(0)) ||
               (obj is Guid && ((Guid)obj) == Guid.Empty)
            ));
        }

        public static bool IsNullTypeOrEmpty(this object obj)
        {
            return (IsNullType(obj) || IsEmpty(obj));
        }

        /// <summary>
        /// Function to save object to external file
        /// http://www.digitalcoding.com/Code-Snippets/C-Sharp/C-Code-Snippet-Save-object-to-file.html
        /// </summary>
        /// <param name="_Object">object to save</param>
        /// <param name="_FileName">File name to save object</param>
        /// <returns>Return true if object save successfully, if not return false</returns>
        public static bool ToFile(this object _Object, string fileName)
        {
            try
            {
                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // Serializes an object, or graph of connected objects, to the given stream.
                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array
                byte[] _ByteArray = _MemoryStream.ToArray();

                // Open file for writing
                System.IO.FileStream _FileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                // Writes a block of bytes to this stream using data from a byte array.
                _FileStream.Write(_ByteArray.ToArray(), 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                // cleanup
                _MemoryStream.Close();
                _MemoryStream.Dispose();
                _MemoryStream = null;
                _ByteArray = null;

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return false;
        }

    }
}
