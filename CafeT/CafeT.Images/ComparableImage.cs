namespace EyeOpen.Imaging
{
    using System;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// Represents an image and its RGB projections.
    /// </summary>
    public class ComparableImage
    {
        private readonly FileInfo file;
        private readonly RgbProjections projections;

        public ComparableImage(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException();
            }

            this.file = file;

            using (var bitmap = ImageUtility.ResizeBitmap(new Bitmap(file.FullName), 100, 100))
            {
                projections = new RgbProjections(ImageUtility.GetRgbProjections(bitmap));
            }
        }

        public FileInfo File
        {
            get 
            { 
                return file; 
            }            
        }

        public RgbProjections Projections
        {
            get 
            { 
                return projections; 
            }            
        }

        /// <summary>
        /// Calculate the similarity to another image.
        /// </summary>
        /// <param name="compare">The image to compare with.</param>
        /// <returns>Return a value from 0 to 1 that is the similarity.</returns>
        public double CalculateSimilarity(ComparableImage compare) 
        {
            return projections.CalculateSimilarity(compare.projections);
        }

        public override string ToString()
        {
            return file.Name;
        }
    }
}
