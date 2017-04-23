using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects
{
    public class FolderObject
    {
        public string FolderPath { set; get; }
        public DirectoryInfo Folder { set; get; }
        public string[] Files { set; get; }
        public long SizeB { set; get; }
        public long SizeKB { set; get; }
        public long SizeMB { set; get; }
        public long SizeGB { set; get; }
        public long SizeTB { set; get; }

        public FolderObject(string folderPath)
        {
            FolderPath = folderPath;
            Folder = new DirectoryInfo(FolderPath);
            Files = Folder.GetFiles().Select(t => t.FullName).ToArray();
            SizeB = GetSize();
            SizeKB = SizeB / 1024;
            SizeMB = SizeKB / 1024;
            SizeGB = SizeMB / 1024;
            SizeTB = SizeGB / 1024;
        }

        protected long GetSize()
        {
            long b = 0;
            foreach (string name in Files)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }
    }
}
