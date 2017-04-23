using CafeT.Writers;
using System;
using System.IO;

namespace CafeT.Watchers
{
    public class FolderWatcher
    {
        private readonly FileSystemWatcher Watcher;
        public static Writer TextWriter { set; get; }

        public FolderWatcher(string folderPath)
        {
            TextWriter = new Writer(folderPath + @"\WatcherDb.txt");

            Watcher = new FileSystemWatcher(folderPath);
            Watcher.Created += watcher_Created;
            Watcher.Deleted += watcher_Deleted;
            Watcher.Renamed += watcher_Renamed;

            //Watcher.Filter = "*.txt";
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.Security | NotifyFilters.Size | NotifyFilters.LastWrite| NotifyFilters.LastAccess;
            Watcher.Changed += watcher_Changed;

            Watcher.Error += watcher_Error;

            Watcher.EnableRaisingEvents = true;
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string _notify = "A change occurred in the monitored directory.Change type: { 0}, file name: { 1}" +  e.ChangeType + e.Name;
            TextWriter.WriteToText(_notify);
            Console.WriteLine();
        }

        static void watcher_Error(object sender, ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException);
            }
        }
        static void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string _notify = "File deleted. Name: {0}" + e.Name;
            TextWriter.WriteToText(_notify);
            Console.WriteLine("File deleted. Name: {0}", e.Name);
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            string _notify = "File created. Name: {0}" + e.Name;
            TextWriter.WriteToText(_notify);
            Console.WriteLine("File created. Name: {0}", e.Name);
        }
        static void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string _notify = "File updated. Old name: {0}, new name: {1}" + e.OldName + e.Name;
            TextWriter.WriteToText(_notify);
            Console.WriteLine("File updated. Old name: {0}, new name: {1}", e.OldName, e.Name);
        }
    }
}
