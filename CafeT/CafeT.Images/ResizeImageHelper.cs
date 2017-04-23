using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Helpers
{
    public class ResizeImageHelper
    {
        //http://expressmagazine.net/development/472/resize-image-thay-doi-kich-thuoc-anh-c
        public Image ResizeImage(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage(img, 0, 0, width, height);
            g.Dispose();

            return (Image)b;
        }
        public Image Resize(Image img, float percentage)
        {
            //lấy kích thước ban đầu của bức ảnh
            int originalW = img.Width;
            int originalH = img.Height;

            //tính kích thước cho ảnh mới theo tỷ lệ đưa vào
            int resizedW = (int)(originalW * percentage);
            int resizedH = (int)(originalH * percentage);

            //tạo 1 ảnh Bitmap mới theo kích thước trên
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //tạo 1 graphic mới từ Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            //vẽ lại ảnh ban đầu lên bmp theo kích thước mới
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //giải phóng tài nguyên mà graphic đang giữ
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }
        public Image resizeImage(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage((Image)b);

            g.InterpolationMode = InterpolationMode.Bicubic;

            // Specify here
            g.DrawImage(img, 0, 0, width, height);
            g.Dispose();

            return (Image)b;
        }
    }

}
