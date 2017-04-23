using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class HomeIndexView
    {
        public List<ProjectModel> LastProjects { set; get; }
        public List<CourseModel> LastCourses { set; get; }
        public List<QuestionModel> LastQuestions { set; get; }
        public List<ArticleView> CreatedNews { set; get; }
        public List<ArticleView> UpdatedNews { set; get;}
        public List<CommentModel> LastComments { set; get; }
        public List<FileModel> LastFiles { set; get; }

        public HomeIndexView()
        {
            LastProjects = new List<ProjectModel>();
            LastCourses = new List<CourseModel>();
            LastQuestions = new List<QuestionModel>();
            CreatedNews = new List<ArticleView>();
            UpdatedNews = new List<ArticleView>();
            LastComments = new List<CommentModel>();
            LastFiles = new List<FileModel>();
        }
    }
}