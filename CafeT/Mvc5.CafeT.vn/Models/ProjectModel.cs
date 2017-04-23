using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ProjectModel:Project
    {
        public ImageModel Image { set; get; }
        public List<ArticleModel> Articles { set; get; }
        public List<QuestionModel> Questions { set; get; }
        public List<FileModel> Files { set; get; }
        public string Tags { set; get; }

        public ProjectModel()
        {
            Files = new List<FileModel>();
            Articles = new List<ArticleModel>();
            Questions = new List<QuestionModel>();
            
        }

        public ProjectModel(string title) : base(title)
        {
            Files = new List<FileModel>();
            Articles = new List<ArticleModel>();
            Questions = new List<QuestionModel>();
            
        }
    }
}