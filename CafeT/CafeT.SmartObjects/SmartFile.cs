using CafeT.Objects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CafeT.SmartObjects
{
    public class SmartFile : BaseObject
    {
        protected readonly static MemoryStream mStream = new MemoryStream();

        public string Content { set; get; }
        public string PathFolder { set; get; }
        //public FolderWatcher Watcher { set; get; }
        public DirectoryInfo FolderInfo { set; get; }
        public string[] Lines { set; get; }

        public string FullPath { set; get; }
        public string Name { set; get; }
        public string RootFolder { set; get; }
        public string Extension { set; get; }


        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] ContentInByte { get; set; }
        public string ContentInString { set; get; }

        public long SizeInB { set; get; }
        public long SizeInKB { set; get; }
        public long SizeInMb { set; get; }
        public long SizeInGb { set; get; }
        public long Size { set; get; }
        public string SizeToString { set; get; }


        protected FileInfo Info { set; get; }

        public SmartFile() : base()
        {
        }

        public SmartFile(string path)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter writer = new StreamWriter(path, false, UnicodeEncoding.UTF8))
                {
                    writer.Close();
                }
            }

            Info = new FileInfo(path);
            FullPath = path;
            PathFolder = new DirectoryInfo(FullPath).Name;
            RootFolder = Path.GetPathRoot(FullPath);
            Extension = Path.GetExtension(FullPath);
            Size = Info.Length;
            Name = Info.Name;
            PathFolder = Info.DirectoryName;
            SizeToString = GetSizeAsString();
            List<string> _lines = new List<string>();

            using (StreamReader reader = new StreamReader(FullPath))
            {
                Content = File.ReadAllText(FullPath);
                Content = reader.ReadToEnd();
                Lines = File.ReadAllLines(FullPath);
                
                while (reader.ReadLine() != null)
                {
                    _lines.Add(reader.ReadLine());
                    reader.Close();
                }
            }

            //StreamReader _reader = new StreamReader(FullPath, UnicodeEncoding.UTF8);
            //Content = File.ReadAllText(FullPath);
            //Content = _reader.ReadToEnd();

            //Lines = File.ReadAllLines(FullPath);
            //List<string> _lines = new List<string>();
            //while (_reader.ReadLine() != null)
            //{
            //    _lines.Add(_reader.ReadLine());
            //    _reader.Close();
            //}

            //if (FullPath != null)
            //{
            //    using (StreamReader reader = new StreamReader(FullPath))
            //    {
            //        foreach (string _line in Lines)
            //        {
            //            Lines.ToList().Add(reader.ReadLine());
            //        }
            //        reader.Close();
            //    }


            //}
        }

        public void AddLineBefore(string line)
        {
            Update();
            List<string> _lines = Lines.ToList();
            _lines.Add(line);
            Lines = _lines.ToArray();
            Content = Content.AddBefore(line);

        }
        public void AddBefore(string line)
        {
            Content = Content.AddBefore(line);
        }

        public void AddAfter(string line)
        {
            Content = Content.AddAfter(line);
        }

        public void Update()
        {
            if (FullPath != null)
            {
                using (StreamReader reader = new StreamReader(FullPath))
                {
                    foreach (string _line in Lines)
                    {
                        Lines.ToList().Add(reader.ReadLine());
                    }
                    reader.Close();
                }


            }
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(FullPath))
            {
                foreach (string _line in Lines)
                {
                    sw.WriteLine(_line);
                }
                sw.Close();
            }
        }


        //--------------------------------------------------------


        #region File Content
        public string FirstPage()
        {
            return string.Empty;
        }

        public string LastPage()
        {
            return string.Empty;
        }

        public string Page(int? i)
        {
            return string.Empty;
        }

        public string[] Page(int begin, int end)
        {
            return new string[] { };
        }

        public int Pages()
        {
            return 0;
        }
        #endregion

        #region Check
        public bool IsExits()
        {
            if (Info == null) return false;
            return true;
        }
        public bool IsImage()
        {
            return false;
        }
        public bool IsWord()
        {
            if (Extension == "doc" || Extension == "docx")
                return true;
            return false;
        }
        public bool IsExcel()
        {
            if (Extension == "xls" || Extension == "xlsx")
                return true;
            return false;
        }
        public bool IsPdf()
        {
            if (Extension == "pdf")
                return true;
            return false;
        }
        public bool IsPowerPoint()
        {
            if (Extension == "ppt" || Extension == "pptx")
                return true;
            return false;
        }
        public bool IsIcon()
        {
            if (Extension == "ico")
                return true;
            return false;
        }
        public bool IsHtml()
        {
            if (Extension == "html" || Extension == "hml")
                return true;
            return false;
        }
        public bool IsZip()
        {
            if (Extension == "zip")
                return true;
            return false;
        }
        public bool IsRar()
        {
            if (Extension == "rar")
                return true;
            return false;
        }
        public bool Is7Zip()
        {
            if (Extension == "7zip")
                return true;
            return false;
        }
        public bool IsTxt()
        {
            if (Extension == "txt")
                return true;
            return false;
        }
        public bool IsExe()
        {
            if (Extension == "exe")
                return true;
            return false;
        }
        public bool IsCSharp()
        {
            if (Extension == "cs")
                return true;
            return false;
        }
        public bool IsVisualBasic()
        {
            if (Extension == "vb")
                return true;
            return false;
        }
        public bool IsJava()
        {
            if (Extension == "java")
                return true;
            return false;
        }
        public bool IsDll()
        {
            if (Extension == "dll")
                return true;
            return false;
        }
        #endregion
        #region Ultilities
        public string GetSizeAsString()
        {
            if (this.SizeInGb > 0) return this.SizeInGb.ToString() + "Gb";
            else if (this.SizeInMb > 0) return this.SizeInMb.ToString() + "Mb";
            else if (this.SizeInKB > 0) return this.SizeInKB.ToString() + "Kb";
            else if (this.SizeInB > 0) return this.SizeInB.ToString() + "B";
            else return string.Empty;
        }

        public string Rename(string newFileName)
        {
            return string.Empty;
        }


        //public void Zip()
        //{
        //    if (!IsExits()) return;

        //    using (ZipFile zip = new ZipFile())
        //    {
        //        // add this map file into the "images" directory in the zip archive
        //        //zip.AddFile(FullPath, Folder);

        //        // add the report into a different directory in the archive
        //        zip.AddFile(FullPath);
        //        //zip.AddFile("ReadMe.txt");
        //        zip.Save(FileName + ".zip");
        //    }
        //}
        #endregion
        #region Actions
        public void Copy(string destPath)
        {
            try
            {
                Info.CopyTo(destPath);
            }
            catch (Exception ex)
            {
                // Call a custom error logging procedure.
                //LogError(e);
                throw new Exception("Can't Copy to " + destPath + ". " + ex.Message);
            }
        }
        public void Move(string destPath)
        {
            try
            {
                Info.MoveTo(destPath);
            }
            catch (Exception ex)
            {
                // Call a custom error logging procedure.
                //LogError(e);
                throw new Exception("Can't Move to " + destPath + ". " + ex.Message);
            }

        }
        public void Delete()
        {
            try
            {
                Info.Delete();
            }
            catch (Exception ex)
            {
                // Call a custom error logging procedure.
                //LogError(e);
                throw new Exception("Can't Delete this file. " + ex.Message);
            }

        }
        public void Encrypt()
        {
            try
            {
                Info.Encrypt();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't Encrypt this file. " + ex.Message);
            }

        }
        public void Decrypt()
        {
            try
            {
                Info.Decrypt();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't Decrypt this file. " + ex.Message);
            }
        }

        //http://www.code-kings.com/2015/08/csharp-rijndael-encryption-decryption.html
        public void Decryption(string inputFile, string outputFile, string Pass)
        {
            try
            {
                string password = Pass; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);
                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                }
                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("You have entered wrong password. " + ex.Message);
            }
        }

        public void Encryption(string inputFile, string outputFile, string Mypassword)
        {
            try
            {
                string password = @Mypassword; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);
                int data;


                while ((data = fsIn.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);

                }
                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Can't Encrypt this file. " + ex.Message);
            }
        }


        #endregion

        #region Prints

        #endregion

    }
}
/// <summary>
/// Author: Svetlin Nakov
/// URL: http://www.nakov.com/blog/2009/07/14/universal-relative-to-physical-path-resolver-for-console-wpf-and-aspnet-apps/
/// </summary>
public class UniversalFilePathResolver
{
    /// <summary>
    /// Resolves a relative path starting with tilde to a physical file system path. In Web application
    /// scenario the "~" denotes the root of the Web application. In desktop application scenario (e.g.
    /// Windows Forms) the "~" denotes the directory where the currently executing assembly is located
    /// excluding "\bin\Debug" and "\bin\Release" folders (if present).
    ///
    /// For example: the path "~\config\example.txt" will be resolved to a physical path like
    /// "C:\Projects\MyProject\config\example.txt".
    ///
    /// </summary>
    /// <param name="relativePath">the relative path to the resource starting with "~"</param>
    /// <returns>Full physical path to the specified resource.</returns>
    public static string ResolvePath(string relativePath)
    {
        if (relativePath == null || !relativePath.StartsWith("~"))
        {
            throw new ArgumentException("The path '" + relativePath +
                "' should be relative path and should start with '~'");
        }

        HttpContext httpContext = HttpContext.Current;
        if (httpContext != null)
        {
            // We are in a Web application --> use Server.MapPath to get the physical path
            string fullPath = httpContext.Server.MapPath(relativePath);
            return fullPath;
        }
        else
        {
            // We are in a console / Windows desktop application -->
            // use currently executing assembly directory to find the full path
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyDir = assembly.CodeBase;
            assemblyDir = assemblyDir.Replace("file:///", "");
            assemblyDir = Path.GetDirectoryName(assemblyDir);

            // Remove "bin\debug" and "bin\release" directories from the path
            string applicationDir = RemoveStringAtEnd(@"\bin\debug", assemblyDir);
            applicationDir = RemoveStringAtEnd(@"\bin\release", applicationDir);

            string fullPath = relativePath.Replace("~", applicationDir);
            return fullPath;
        }
    }

    private static string RemoveStringAtEnd(string searchStr, string targetStr)
    {
        if (targetStr.ToLower().EndsWith(searchStr.ToLower()))
        {
            string resultStr = targetStr.Substring(0, targetStr.Length - searchStr.Length);
            return resultStr;
        }
        return targetStr;
    }
}