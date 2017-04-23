using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class CommentView : BaseView
    {
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Summary { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        public Guid? ArticleId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }

        public int? Points { set; get; }

        public bool IsPublished { set; get; }
        public bool IsDrafted { set; get; }
        public bool IsPublic { set; get; }
        public bool IsProtect { set; get; }
        public bool IsPrivate { set; get; }
        public string Followers { set; get; }

        public int CountViews { set; get; }
        
        public virtual ArticleModel Article { set; get; }

        public List<FileModel> Files { set; get; }

        

        public string Tags { set; get; }

        public ApplicationUser Author { set; get; }
        public string ImageAuthor { set; get; }

        public List<ImageModel> ImageModels { get; set; }

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public CommentView()
        {
            Files = new List<FileModel>();
            Author = new ApplicationUser();
            ImageModels = new List<ImageModel>();
        }

        public bool HasMeaning()
        {
            if (this.Title.GetCountWords() <= 3) return false;
            if (this.Content.GetCountWords() <= 10) return false;

            return true;
        }

        public bool CanView()
        {
            if (Title.ToWords().Length <= 2)
            {
                return false;
            }
            if (Content.ToWords().Length <= 2) return false;
            return true;
        }

        public bool IsNews()
        {
            if (this.CreatedDate.AddDays(3) >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        //public bool IsUpdated()
        //{
        //    if (this.CreatedDate != this.LastUpdatedDate)
        //    {
        //        return true;
        //    }
        //    return false;
        //}        
    }
}