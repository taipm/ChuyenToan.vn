using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EyeOpen.Imaging;
using LicensePlateRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Images
{
    public enum ImageType
    {
        jpg,
        jpeg,
    };

    public class ImageManager //: IImageManager
    {
        public string FullPath { set; get; }
        public List<FileInfo> Files { set; get; }
        public List<Image> Images { set; get; }
        public List<ComparableImage> CompareableImages { set; get; }
        public SimilarityImages Similarity { set; get; }
        public ImageManager(string folderPath)
        {
            FullPath = folderPath;
            CompareableImages = new List<ComparableImage>();
            Files = new List<FileInfo>();
            Files = LoadImages(ImageType.jpg).ToList();
            Images = new List<Image>();
        }

        public FileInfo[] LoadImages(ImageType type)
        {
            DirectoryInfo directoryInfo;
            try
            {
                directoryInfo = new DirectoryInfo(FullPath);
                return directoryInfo.GetFiles("*." + type.ToString(), SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

       
            /// <summary>
            /// Compare and return list of results with Similarity images compare with source
            /// </summary>
            /// <param name="file"></param>
            /// <param name="files"></param>
            /// <returns></returns>
            /// 
        public List<SimilarityImages> GetSimilarityImages(FileInfo file)
        {
            ComparableImage _source = new ComparableImage(file);
            var comparableImages = new List<ComparableImage>();
            var similarityImagesSorted = new List<SimilarityImages>();

            var index = 0x0;

            foreach (var _file in Files)
            {
                var comparableImage = new ComparableImage(_file);
                comparableImages.Add(comparableImage);
                index++;
            }


            index = 0;

            for (var i = 0; i < comparableImages.Count - 1; i++)
            {
                var destination = comparableImages[i];
                var similarity = _source.CalculateSimilarity(destination);
                var sim = new SimilarityImages(_source, destination, similarity);

                similarityImagesSorted.Add(sim);
                index++;
            }
            similarityImagesSorted.Sort();
            similarityImagesSorted.Reverse();
            return similarityImagesSorted;
        }

        //private LicensePlateDetector _licensePlateDetector;
        IInputOutputArray image;

        public List<string> ExtractText(string path)
        {
            List<IInputOutputArray> licensePlateImagesList = new List<IInputOutputArray>();
            List<IInputOutputArray> filteredLicensePlateImagesList = new List<IInputOutputArray>();
            List<RotatedRect> licenseBoxList = new List<RotatedRect>();
            LicensePlateDetector _licensePlateDetector = new LicensePlateDetector("");

            Mat m = new Mat(path, LoadImageType.AnyColor);
            UMat um = m.ToUMat(AccessType.ReadWrite);

            List<string> words = _licensePlateDetector.DetectLicensePlate(
               m,
               licensePlateImagesList,
               filteredLicensePlateImagesList,
               licenseBoxList);
            return words;
        }
    }
}
