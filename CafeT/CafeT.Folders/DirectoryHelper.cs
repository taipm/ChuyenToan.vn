using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Folders
{
    public static class DirectoryHelper
    {
        public static long FolderSize(this DirectoryInfo dir, bool bIncludeSub)
        {
            long totalFolderSize = 0;

            if (!dir.Exists) return 0;

            var files = from f in dir.GetFiles()
                        select f;
            foreach (var file in files) totalFolderSize += file.Length;

            if (bIncludeSub)
            {
                var subDirs = from d in dir.GetDirectories()
                              select d;
                foreach (var subDir in subDirs) totalFolderSize += FolderSize(subDir, true);
            }

            return totalFolderSize;
        }
    }
}
