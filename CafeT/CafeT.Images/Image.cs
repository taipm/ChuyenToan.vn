using CafeT.Objects;
using EyeOpen.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Images
{
    public class Image:BaseObject
    {
        public string Name { set; get; }
        public string FullPath { set; get; }
        protected FileInfo file;

        public Image()
        {
        }
        public Image(string fullPath)
        {
            FullPath = fullPath;
            file = new FileInfo(fullPath);
        }

        public string ExtractText()
        {
            return string.Empty;
        }

        public Bitmap LoadAsBitmap()
        {
            var _bmp = (Bitmap)System.Drawing.Image.FromFile(FullPath);
            return _bmp;
        }

        public ComparableImage ToCompareImage()
        {
            return new ComparableImage(this.file);
        }
        
    }
}
