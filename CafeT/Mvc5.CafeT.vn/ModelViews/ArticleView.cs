using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ArticleAuthorView
    {
        public ApplicationUser UserView { set; get; }
        public ArticleAuthorView()
        {
            UserView = new ApplicationUser();
        }
    }

    public class ArticleView:BaseView
    {
        [Display(Name = "Tiêu đề")]
        public string Title { set; get; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Summary { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        public int? Points { set; get; }

        public bool IsPublished { set; get; }
        public bool IsDrafted { set; get; }
        public bool IsPublic { set; get; }
        public bool IsProtect { set; get; }
        public bool IsPrivate { set; get; }
        public string Followers { set; get; }

        public int CountViews { set; get; }
        [Display(Name = "Nội dung Tiếng Anh")]
        public string EnglishContent { set; get; }
        [Display(Name = "Nội dung")]
        public string VietnameseContent { set; get; }
        public CommentModel Comment { set; get; }
        public QuestionModel QuestionCreate { set; get; }

        public List<CommentModel> Comments { set; get; }
        public List<QuestionModel> Questions { set; get; }

        public List<FileModel> Files { set; get; }

        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }

        public string Tags { set; get; }

        public ApplicationUser Author { set; get; }
        public string ImageAuthor { set; get; }

        public List<ImageModel> ImageModels { get; set; }

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        public ArticleView()
        {
            Files = new List<FileModel>();
            Author = new ApplicationUser();
            ImageModels = new List<ImageModel>();
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
            if(this.CreatedDate.AddDays(3) >= DateTime.Now)
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