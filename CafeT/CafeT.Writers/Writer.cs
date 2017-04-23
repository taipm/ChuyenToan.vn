using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Writers
{
    public class Writer
    {
        public string PathTextDb;

        public Writer(string pathTextDb)
        {
            PathTextDb = pathTextDb;
            StreamWriter _write = new StreamWriter(PathTextDb);
            string _newLine = "/*" + "ComputerManager" + "*/";
            _write.WriteLine(_newLine);
            _write.Close();
        }

        public void WriteToText(string text)
        {
            StreamWriter _write = new StreamWriter(PathTextDb, true, UTF8Encoding.UTF8);
            string _newLine = "/*" + DateTime.Now.ToShortDateString() + "*/";
            _write.WriteLine(_newLine);
            _write.Write(text);
            _write.Close();
        }

        public void WriteToText(string path, string text)
        {
            StreamWriter _write = new StreamWriter(PathTextDb, true, UTF8Encoding.UTF8);
            string _newLine = "/*" + DateTime.Now.ToShortDateString() + "*/";
            _write.WriteLine(_newLine);
            _write.Write(text);
            _write.Close();
        }

        public void WriteToText(string path, List<string> list)
        {
            //if (File.Exists(path)) File.Delete(path);
            StreamWriter _write = new StreamWriter(PathTextDb, true, UTF8Encoding.UTF8);
            foreach (string item in list)
            {
                _write.WriteLine(item);
            }
            _write.Close();
        }
    }
}
