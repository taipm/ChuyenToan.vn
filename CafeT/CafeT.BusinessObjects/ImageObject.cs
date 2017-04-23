using CafeT.Objects;
using EyeOpen.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects
{
    public class ImageObject:BaseObject
    {
        public string FileName { set; get; }
        public string Description { set; get; }
        public string FullPath { set; get; }
        protected FileInfo file;

        public ImageObject()
        {
        }
        public ImageObject(string fullPath)
        {
            FullPath = fullPath;
            file = new FileInfo(fullPath);
            FileName = file.Name;
        }

        public string ExtractText()
        {
            return string.Empty;
        }

        //public Bitmap LoadAsBitmap()
        //{
        //    var _bmp = (Bitmap)System.Drawing.Image.FromFile(FullPath);
        //    return _bmp;
        //}

        public ComparableImage ToCompareImage()
        {
            return new ComparableImage(this.file);
        }
    }
}
