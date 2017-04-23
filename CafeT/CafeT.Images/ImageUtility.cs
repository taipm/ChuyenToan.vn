namespace EyeOpen.Imaging
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// An utility class to do simple image processing.
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// Resize an image in high resolution
        /// </summary>
        /// <param name="bitmap">The image to resize.</param>
        /// <param name="width">The expected width.</param>
        /// <param name="height">the expected height.</param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        {
            var result = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage((System.Drawing.Image)result))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.DrawImage(bitmap, 0, 0, width - 1, height - 1);
            }

            return result;
        }

        /// <summary>
        /// Calculate the RBG projection.
        /// </summary>
        /// <param name="bitmap">The image to process.</param>
        /// <returns>Return horizontal RGB projection in value [0] and vertical RGB projection in value [1].</returns>
        public static double[][] GetRgbProjections(Bitmap bitmap)
        {
            var width = bitmap.Width - 1;
            var height = bitmap.Width - 1;
            
            var horizontalProjection = new double[width];
            var verticalProjection = new double[height];

            var bitmapData1 = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe //Must setup allow unsafe code in Project Property - TAIPM comment
            {
                var imagePointer1 = (byte*)bitmapData1.Scan0;

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var blu = imagePointer1[0];
                        var green = imagePointer1[1];
                        var red = imagePointer1[2];

                        int luminosity = (byte)(((0.2126 * red) + (0.7152 * green)) + (0.0722 * blu));

                        horizontalProjection[x] += luminosity;
                        verticalProjection[y] += luminosity;

                        imagePointer1 += 4;
                    } 
                    
                    imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);
                }
            }

            MaximizeScale(ref horizontalProjection, height);
            MaximizeScale(ref verticalProjection, width);

            var projections = 
                new[]
                    {
                        horizontalProjection, 
                        verticalProjection
                    };

            bitmap.UnlockBits(bitmapData1);
            return projections;
        }

        /// <summary>
        /// Optimize the range of values.
        /// </summary>
        /// <param name="projection">The array to process.</param>
        /// <param name="max">The max value for the elements.</param>
        private static void MaximizeScale(ref double[] projection, double max)
        {
            var minValue = double.MaxValue;
            var maxValue = double.MinValue;

            for (var i = 0; i < projection.Length; i++) 
            {
                if (projection[i] > 0)
                {
                    projection[i] = projection[i] / max;
                }

                if (projection[i] < minValue)
                {
                    minValue = projection[i];
                }
                
                if (projection[i] > maxValue)
                {
                    maxValue = projection[i];
                }
            }

            if (maxValue == 0)
            {
                return;
            }

            for (var i = 0; i < projection.Length; i++)
            {
                if (maxValue == 255)
                {
                    projection[i] = 1;
                }
                else
                {
                    projection[i] = (projection[i] - minValue) / (maxValue - minValue);
                }
            }
        }

        public static Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var memStream = new MemoryStream(imageBytes, 0, imageBytes.Length);

            memStream.Write(imageBytes, 0, imageBytes.Length);
            var image = System.Drawing.Image.FromStream(memStream);
            return image;
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}