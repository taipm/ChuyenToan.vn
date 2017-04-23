using System;
using System.Collections.Generic;
using CafeT.BusinessObjects;
using CafeT.Text;
using CafeT.Html;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.Models
{
    public class ArticleCollection:List<ArticleModel>
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
    }

    public class ArticleModel : Article
    {
        public string EnglishContent { set; get; }
        public string VietnameseContent { set; get; }
        public int? Points { set; get; }

        public virtual ImageModel Avatar { set; get; }

        public Guid? CourseId { set; get; }
        
        public Guid? ProjectId { set; get; }

        public Guid? CompanyId { set; get; }

        public Guid? JobId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }
        public virtual ArticleCategory Category { set; get; }

        public string Followers { set; get; }

        public string FromUrl { set; get; }

        public IEnumerable<QuestionModel> Questions { set; get; }
        public IEnumerable<CommentModel> Comments { set; get; }
        public IEnumerable<FileModel> Files { set; get; }

        public ArticleModel():base()
        {
            Points = 0;
        }

        public ArticleModel(string title)
        {
        }

        public ArticleModel ToStandard()
        {
            Title = Title.ToStandard();
            Summary = Summary.ToStandard();
            Content = Content.ToStandard();
            return this;
        }

        public Dictionary<string, string> GetWarning()
        {
            return new Dictionary<string, string>();
        }

        public bool HasWarning()
        {
            var _warnings = GetWarning();
            if(_warnings.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void Save(string pathFolder)
        {
            if(this.Content != null)
            {
                SmartHtml _smartHtml = new SmartHtml(this.Content);
                string _fileName = this.Title.HtmlToText().Replace(":","");
                try
                {
                    _smartHtml.SaveAsHtml(pathFolder + @"\" + _fileName + ".html", System.Text.UnicodeEncoding.UTF8);
                }
                catch(Exception ex)
                {
                    throw new Exception("Can't save this file. " + ex.Message);
                }
            }
        }
    }
}