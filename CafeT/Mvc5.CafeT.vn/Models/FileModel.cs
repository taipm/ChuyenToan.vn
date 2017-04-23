using System;
using CafeT.BusinessObjects;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.Models
{
    public class FileModel : FileObject
    {
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        public string Tags { set; get; }
        
        public int? CountDownloads { set; get; }
        public string SizeToString { set;get; }

        public ImageObject Image { set; get; }

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public FileModel():base()
        {
        }

        public FileModel(string fullPath) : base(fullPath)
        {
            SizeToString = base.GetSizeAsString();
            CountDownloads = 0;
        }

        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? AnswerId { set; get; }

        public bool IsImage()
        {
            string[] _imgExts = new string[] { ".png", ".jpg"};
            string _ext = this.GetExtension();
            if (_imgExts.ToList().Contains(_ext)) return true;
            return false;
        }

        public bool IsZiped()
        {
            string[] _imgExts = new string[] { ".zip", ".rar", ".7zip" };
            string _ext = this.GetExtension();
            if (_imgExts.ToList().Contains(_ext)) return true;
            return false;
        }
    }
}