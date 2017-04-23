using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects
{
    public enum FileType
    {
        Avatar = 1,
        Pdf = 2,
        Zip = 3,
        Rar = 4,
        Word = 5,
        Photo
    }

    public class FileObject
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }

        public string FullPath { set; get; }
        public string FileName { get; set; }
        public string Root { get; set; }
        public string Folder { get; set; }
        public string Extension { set; get; }
        public long SizeInB { set; get; }
        public long SizeInKB { set; get; }
        public long SizeInMb { set; get; }
        public long SizeInGb { set; get; }

        public FileObject() { }

        public FileObject(string fullPath)
        {
            if (!IsExits()) return;
            FullPath = fullPath;
            FileName = GetFileName();
            Folder = GetFolder();
            Extension = GetExtension();
            Root = GetRoot();
            SizeInB = GetSize();
            SizeInKB = SizeInB / 1024;
            SizeInMb = SizeInKB / 1024;
            SizeInGb = SizeInMb / 1024;
        }

        public long GetSize()
        {
            FileInfo _fi = new FileInfo(FullPath);
            return _fi.Length;
        }

        public string GetRoot()
        {
            string _extension = Path.GetPathRoot(FullPath);
            return _extension;
        }
        public string GetExtension()
        {
            string _extension = Path.GetExtension(FullPath);
            return _extension;
        }

        public string GetFileName()
        {

            string _filename = Path.GetFileName(FullPath);
            return _filename;
        }

        public string GetFolder()
        {
            string _dirName = new DirectoryInfo(FullPath).Name;
            return _dirName;
        }

        public string Rename(string newFileName)
        {
            return string.Empty;
        }

        public bool IsExits()
        {
            if (File.Exists(FullPath)) return true;
            return false;
        }

        public void Zip()
        {
            if (!IsExits()) return;

            using (ZipFile zip = new ZipFile())
            {
                // add this map file into the "images" directory in the zip archive
                //zip.AddFile(FullPath, Folder);

                // add the report into a different directory in the archive
                zip.AddFile(FullPath);
                //zip.AddFile("ReadMe.txt");
                zip.Save(FileName + ".zip");
            }
        }
    }
}
