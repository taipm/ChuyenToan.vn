using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Shared
{
    class Utils
    {
        public string OrignalFileName { get; set; }
        public string FileName { set; get; }
        public string TempFolder { get; set; }
        public int MaxFileSizeMB { get; set; }
        public List<String> FileParts { get; set; }

        public Utils(string fileName)
        {
            MaxFileSizeMB = 1;
            FileParts = new List<string>();
            OrignalFileName = fileName;
        }
        /// <summary>
        /// Split = get number of files 
        /// .. Name = original name + ".part_N.X" (N = file part number, X = total files)
        /// </summary>
        /// <returns></returns>
        public bool SplitFile(string fileName)
        {
            // improvement - make more robust
            bool rslt = false;
            string BaseFileName = Path.GetFileName(fileName);
            int BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
            const int READBUFFER_SIZE = 1024;
            byte[] FSBuffer = new byte[READBUFFER_SIZE];
            // adapted from: http://stackoverflow.com/questions/3967541/how-to-split-large-files-efficiently
            using (FileStream FS = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int TotalFileParts = 0;
                if (FS.Length < BufferChunkSize)
                {
                    TotalFileParts = 1;
                }
                else
                {
                    float PreciseFileParts = ((float)FS.Length / (float)BufferChunkSize);
                    TotalFileParts = (int)Math.Ceiling(PreciseFileParts);
                }

                int FilePartCount = 0;
                while (FS.Position < FS.Length)
                {
                    string FilePartName = String.Format("{0}.part_{1}.{2}", BaseFileName, (FilePartCount + 1).ToString(), TotalFileParts.ToString());
                    FilePartName = Path.Combine(TempFolder, FilePartName);
                    FileParts.Add(FilePartName);
                    using (FileStream FilePart = new FileStream(FilePartName, FileMode.Create))
                    {
                        int bytesRemaining = BufferChunkSize;
                        int bytesRead = 0;
                        while (bytesRemaining > 0 && (bytesRead = FS.Read(FSBuffer, 0, Math.Min(bytesRemaining, READBUFFER_SIZE))) > 0)
                        {
                            FilePart.Write(FSBuffer, 0, bytesRead);
                            bytesRemaining -= bytesRead;
                        }
                    }
                    FilePartCount++;
                }

            }
            return rslt;
        }

        /// <summary>
        /// original name + ".part_N.X" (N = file part number, X = total files)
        /// Objective = enumerate files in folder, look for all matching parts of split file. If found, merge and return true.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool MergeFile(string FileName)
        {
            bool rslt = false;
            // parse out the different tokens from the filename according to the convention
            string partToken = ".part_";
            string baseFileName = FileParts[0].Substring(0, FileParts[0].IndexOf(partToken));
            string trailingTokens = FileParts[0].Substring(FileParts[0].IndexOf(partToken) + partToken.Length);
            int FileIndex = 0;
            int FileCount = 0;
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
            int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out FileCount);
            // get a list of all file parts in the temp folder
            string Searchpattern = Path.GetFileName(baseFileName) + partToken + "*";
            string[] FilesList = Directory.GetFiles(Path.GetDirectoryName(OrignalFileName), Searchpattern);
            //  merge .. improvement would be to confirm individual parts are there / correctly in sequence, a security check would also be important
            // only proceed if we have received all the file chunks
            if (FilesList.Count() == FileCount) 
            {
                // use a singleton to stop overlapping processes
                if (!MergeFileManager.Instance.InUse(baseFileName))
                {
                    MergeFileManager.Instance.AddFile(baseFileName);
                    if (File.Exists(baseFileName))
                        File.Delete(baseFileName);
                    // add each file located to a list so we can get them into 
                    // the correct order for rebuilding the file
                    List<SortedFile> MergeList = new List<SortedFile>();
                    foreach (string File in FilesList)
                    {
                        SortedFile sFile = new SortedFile();
                        sFile.FileName = File;
                        baseFileName = File.Substring(0, File.IndexOf(partToken));
                        trailingTokens = File.Substring(File.IndexOf(partToken) + partToken.Length);
                        int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
                        sFile.FileOrder = FileIndex;
                        MergeList.Add(sFile);
                    }
                    // sort by the file-part number to ensure we merge back in the correct order
                    var MergeOrder = MergeList.OrderBy(s => s.FileOrder).ToList(); 
                    using (FileStream FS = new FileStream(baseFileName, FileMode.Create))
                    {
                        // merge each file chunk back into one contiguous file stream
                        foreach (var chunk in MergeOrder)
                        {
                            try
                            {
                                using (FileStream fileChunk = new FileStream(chunk.FileName, FileMode.Open))
                                {
                                    fileChunk.CopyTo(FS);
                                }
                            }
                            catch (IOException ex)
                            {
                                // handle                                
                            }
                        }
                    }
                    rslt = true;
                    // unlock the file from singleton
                    MergeFileManager.Instance.RemoveFile(baseFileName);
                }
            }
            foreach(var filePart in FileParts)
            {
                if(File.Exists(filePart))
                {
                    File.Delete(filePart);
                }
            }
            
            return rslt;
        }


    }

    public struct SortedFile
    {
        public int FileOrder { get; set; }
        public String FileName { get; set; }
    }

    public class MergeFileManager
    {
        private static MergeFileManager instance;
        private List<string> MergeFileList;

        private MergeFileManager()
        {
            try
            {
                MergeFileList = new List<string>();
            }
            catch { }
        }

        public static MergeFileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MergeFileManager();
                return instance;
            }
        }

        public void AddFile(string BaseFileName)
        {
            MergeFileList.Add(BaseFileName);
        }

        public bool InUse(string BaseFileName)
        {
            return MergeFileList.Contains(BaseFileName);
        }

        public bool RemoveFile(string BaseFileName)
        {
            return MergeFileList.Remove(BaseFileName);
        }
    }

}



