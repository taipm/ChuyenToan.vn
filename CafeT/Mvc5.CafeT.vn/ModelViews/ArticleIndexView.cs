using CafeT.BusinessObjects;
using CafeT.BusinessObjects.ELearning;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ArticleIndexView
    {
        public List<Article> CreatedNews { set; get; }
        public List<Article> UpdatedNews { set; get;}

        public string Title { set; get; }
        public string Summary { set; get; }
        public string Content { set; get; }

        public CommentModel Comment { set; get; }
        public QuestionModel QuestionCreate { set; get; }

        public List<CommentModel> Comments { set; get; }
        public List<QuestionModel> Questions { set; get; }

        public List<FileModel> Files { set; get; }

        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }

        public string Tags { set; get; }

        public ArticleIndexView()
        {
            Files = new List<FileModel>();
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
    }
}