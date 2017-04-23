using System.Collections.Generic;
using System.Linq;
using System.IO;
using CafeT.Writers;
using CafeT.Watchers;

namespace CafeT.SmartObjects
{
    public class SmartFolder
    {
        public string PathFolder { set; get; }
        public FolderWatcher Watcher { set; get; }
        public bool EnableWatcher { set; get; }
        public DirectoryInfo FolderInfo { set; get; }
        public FileInfo[] Files { set; get; }


        public SmartFolder()
        {
            EnableWatcher = false;
        }

        public SmartFolder(string pathFolder, bool? isWatcher)
        {
            PathFolder = pathFolder;
            if (isWatcher == null)
            {
                EnableWatcher = false;
            }
            else
            {
                EnableWatcher = isWatcher.Value;
            }
            
            if(EnableWatcher)
            {
                Watcher = new FolderWatcher(PathFolder);
            }
            FolderInfo = new DirectoryInfo(PathFolder);
            Files = this.GeFiles(true);
        }

        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public FileInfo[] GeFiles(bool hasSub)
        {
            if(!hasSub)
            {
                return FolderInfo.GetFiles();
            }
            else
            {
                return FolderInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToArray();
            }
        }

        public SmartFile[] GeSmartFiles(bool hasSub)
        {
            List<SmartFile> _smartFiles = new List<SmartFile>();
            if (!hasSub)
            {
                var _files = FolderInfo.GetFiles();
                foreach (var _file in _files)
                {
                    var _smartFile = new SmartFile(_file.FullName);
                    _smartFiles.Add(_smartFile);
                }

                return _smartFiles.ToArray();
            }
            else
            {
                var _files = FolderInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToArray();
                foreach (var _file in _files)
                {
                    var _smartFile = new SmartFile(_file.FullName);
                    _smartFiles.Add(_smartFile);
                }

                return _smartFiles.ToArray();
            }
        }

        public FileInfo[] GetAllFiles()
        {
            return FolderInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).ToArray();
        }

        public void WriteFileInfoToText(string path)
        {
            new Writer(path).WriteToText(path, Files.Select(t=>t.Name).ToList());
        }
        public void SyncTo(string pathDestination)
        {

        }
    }
}
