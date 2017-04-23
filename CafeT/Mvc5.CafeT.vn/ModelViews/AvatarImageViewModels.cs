using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class AvatarImageViewModel
    {
        public AvatarImageViewModel()
        {
            Images = new List<string>();
        }

        public List<string> Images { get; set; }
    }

    public class AvatarUploadImageViewModel
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
}