using CafeT.BusinessObjects;
using CafeT.GenericList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ImageModel:ImageObject
    {
        public Guid? ArticleId { set; get; }
        public Guid? FileId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        
        public ImageModel()
        {
        }
        public ImageModel(string fullPath):base(fullPath)
        {
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public string ViewAsRazorHtml()
        {
            //string _img = "< img src = "@Url.Content(item.FullPath)" alt = "@item.Name" width = "100" height = "100" />";
            return string.Empty;
        }
    }
    //Fancy Upload
    public class ImagesModel
    {
        public ImagesModel()
        {
            Images = new List<string>();
        }

        public List<string> Images { get; set; }
    }

    public class UploadImageModel
    {
        [Display(Name = "Internet URL")]
        public string Url { get; set; }

        public bool IsUrl { get; set; }

        [Display(Name = "Flickr image")]
        public string Flickr { get; set; }

        public bool IsFlickr { get; set; }

        [Display(Name = "Local file")]
        public HttpPostedFileBase File { get; set; }

        public bool IsFile { get; set; }

        [Range(0, int.MaxValue)]
        public int X { get; set; }

        [Range(0, int.MaxValue)]
        public int Y { get; set; }

        [Range(1, int.MaxValue)]
        public int Width { get; set; }

        [Range(1, int.MaxValue)]
        public int Height { get; set; }
    }

    public class ImageModels:GenericList<ImageModel>
    {
        public string ErrorMsg = string.Empty;

        public List<ImageModel> Sort(ImageModels list)
        {
            return list.OrderBy(t => t.FileName).ToList();
        }

        public ImageModel Next(ImageModel model)
        {
            try
            {
                return this.Items[IndexOf(model) + 1];
            }
            catch(Exception ex)
            {
                ErrorMsg = ex.Message;
                return null;
            }
        }
    }
}