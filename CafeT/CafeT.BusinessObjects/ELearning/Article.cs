using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeT.BusinessObjects
{
    public class Article:BaseObject
    { 
        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Tóm tắt")]
        public string Summary { set; get; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { set; get; }
        [Display(Name = "Tags")]
        public string Tags { set; get; }
        public List<string> Authors { set; get; }

        public bool IsPublished {set; get; }
        public bool IsDrafted { set; get; }
        public bool IsProtect { set; get; }
        public bool IsPublic { set; get; }
        public bool IsPrivate { set; get; }

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public Article():base()
        {
            Authors = new List<string>();
            Authors.Add(CreatedBy);

            IsDrafted = true;
            IsPrivate = true;

            IsPublished = false;
            IsProtect = false;
            IsPublic = false;
        }

        public void ToDraft()
        {
            this.IsPublished = false;
            this.IsDrafted = true;
        }

        public void ToPublish()
        {
            this.IsDrafted = false;
            this.IsPublished = true;
        }

        public void ToProtect()
        {
            this.IsProtect= true;
            this.IsPrivate = false;
            this.IsPublic = false;
        }

        public void ToPrivate()
        {
            this.IsProtect = false;
            this.IsPrivate = true;
            this.IsPublic = false;
        }

        public void ToPublic()
        {
            this.IsProtect = false;
            this.IsPrivate = false;
            this.IsPublic = true;
        }
    }
}
