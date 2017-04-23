using Mvc5.CafeT.vn.ModelViews;
using System.Collections.Generic;
using System.Linq;
using CafeT.Text;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Services;
using Microsoft.AspNet.Identity;

namespace Mvc5.CafeT.vn.Mappers
{
    public class Mappers
    {
        protected readonly ArticleManager _manager;

        public Mappers(IUnitOfWorkAsync unitOfWorkAsync,
            IArticleService service)
        {
            _manager = new ArticleManager(service, unitOfWorkAsync);
        }

        public ArticleModel ToArticle(WebPageModel model)
        {
            ArticleModel _view = new ArticleModel();
            _view.Id = model.Id;
            _view.CreatedDate = model.CreatedDate;
            _view.CreatedBy = model.CreatedBy;
            _view.Title = model.Title;
            _view.Content = model.Page.HtmlSmart.HtmlContent;
            
            if(model.Title != null)
            {
                _view.Tags = model.Title.ToWords().First();
            }
            else
            {
                _view.Tags = string.Empty;
            }
            

            return _view;
        }
        public  ArticleView ToView(ArticleModel model)
        {
            ArticleView _view = new ArticleView();
            _view.Id = model.Id;
            _view.ProjectId = model.ProjectId;
            _view.CourseId = model.CourseId;
            _view.CreatedDate = model.CreatedDate;
            _view.CreatedBy = model.CreatedBy;
            _view.LastUpdatedBy = model.LastUpdatedBy;
            _view.LastUpdatedDate = model.LastUpdatedDate;
            _view.Title = model.Title;
            _view.IsDrafted = model.IsDrafted;
            _view.IsPrivate = model.IsPrivate;
            _view.IsProtect = model.IsProtect;
            _view.IsPublic = model.IsPublic;
            _view.IsPublished = model.IsPublished;
            _view.Followers = model.Followers;
            _view.CategoryId = model.CategoryId;
            _view.Summary = model.Summary;
            _view.Content = model.Content;
            _view.CountViews = model.CountViews;
            _view.Points = model.Points;
            _view.EnglishContent = model.EnglishContent;
            _view.VietnameseContent = model.VietnameseContent;
            _view.AvatarPath = model.AvatarPath;            

            if (model.Tags != null)
            {
                _view.Tags = model.Tags;
            }
            else
            {
                _view.Tags = model.Title.ToWords().First();
            }
            
            _view.Files = _manager.GetFiles(model.Id).ToList();
            var _comment = new CommentModel();
            _comment.ArticleId = model.Id;
            _comment.Title = "Reply on [" + model.Title + "]";
            _view.Comment = _comment;

            _view.QuestionCreate = new QuestionModel();
            _view.QuestionCreate.ArticleId = model.Id;
            _view.Questions = _manager.GetQuestions(model.Id).ToList();
            if(model.Comments != null && model.Comments.Count()>0)
            {
                _view.Comments = model.Comments.ToList();
            }
            else
            {
                _view.Comments = new List<CommentModel>();
            }
            return _view;
        }

        public ExamView ToView(ExamModel model)
        {
            ExamView _view = new ExamView();
            _view.Id = model.Id;
            _view.CreatedDate = model.CreatedDate;
            _view.CreatedBy = model.CreatedBy;
            _view.Name = model.Name;
            _view.Description = model.Description;
            _view.QuestionCreate = new QuestionModel();
            _view.QuestionCreate.ExamId = model.Id;
            _view.CountViews = model.CountViews;
            return _view;
        }

        public List<ArticleView> ToViews(List<ArticleModel> models)
        {
            List<ArticleView> _views = new List<ArticleView>();
            foreach(var model in models)
            {
                _views.Add(ToView(model));
            }
            return _views.ToList();
        }

        public ArticleModel ToModel(ArticleView view)
        {
            ArticleModel _model = new ArticleModel();
            _model.Id = view.Id;
            _model.ProjectId = view.ProjectId;
            _model.CourseId = view.CourseId;
            _model.Tags = view.Tags;
            _model.CreatedBy = view.CreatedBy;
            _model.CreatedDate = view.CreatedDate;
            _model.LastUpdatedDate = view.LastUpdatedDate;
            _model.LastUpdatedBy = view.LastUpdatedBy;
            _model.Title = view.Title;
            _model.Summary = view.Summary;
            _model.Content = view.Content;
            _model.EnglishContent = view.EnglishContent;
            _model.VietnameseContent = view.VietnameseContent;
            _model.Followers = view.Followers;
            _model.CategoryId = view.CategoryId;
            _model.IsDrafted = view.IsDrafted;
            _model.IsPrivate = view.IsPrivate;
            _model.IsProtect = view.IsProtect;
            _model.IsPublic = view.IsPublic;
            _model.IsPublished = view.IsPublished;
            _model.AvatarPath = view.AvatarPath;
            _model.CountViews = view.CountViews;

            return _model;
        }
        public CommentModel ToModel(CommentView view)
        {
            CommentModel _model = new CommentModel();
            _model.Id = view.Id;
            _model.ArticleId = view.ArticleId;
            _model.ProjectId = view.ProjectId;
            _model.CourseId = view.CourseId;            
            _model.CreatedBy = view.CreatedBy;
            _model.CreatedDate = view.CreatedDate;
            _model.LastUpdatedDate = view.LastUpdatedDate;
            _model.LastUpdatedBy = view.LastUpdatedBy;
            _model.Title = view.Title;            
            _model.Content = view.Content;
            //_model.EnglishContent = view.EnglishContent;
            //_model.VietnameseContent = view.VietnameseContent;
            //_model.Followers = view.Followers;
            //_model.CategoryId = view.CategoryId;
            //_model.IsDrafted = view.IsDrafted;
            //_model.IsPrivate = view.IsPrivate;
            //_model.IsProtect = view.IsProtect;
            //_model.IsPublic = view.IsPublic;
            //_model.IsPublished = view.IsPublished;
            //_model.AvatarPath = view.AvatarPath;
            _model.CountViews = view.CountViews;

            return _model;
        }
        public CommentView ToView(CommentModel model)
        {
            CommentView _view = new CommentView();
            _view.Id = model.Id;
            _view.ProjectId = model.ProjectId;
            _view.CourseId = model.CourseId;
            _view.CreatedDate = model.CreatedDate;
            _view.CreatedBy = model.CreatedBy;
            _view.LastUpdatedBy = model.LastUpdatedBy;
            _view.LastUpdatedDate = model.LastUpdatedDate;
            _view.Title = model.Title;
            //_view.IsDrafted = model.IsDrafted;
            //_view.IsPrivate = model.IsPrivate;
            //_view.IsProtect = model.IsProtect;
            //_view.IsPublic = model.IsPublic;
            //_view.IsPublished = model.IsPublished;
            //_view.Followers = model.Followers;
            //_view.CategoryId = model.CategoryId;
            //_view.Summary = model.Summary;
            _view.Content = model.Content;
            _view.CountViews = model.CountViews;
            //_view.Points = model.Points;
            //_view.EnglishContent = model.EnglishContent;
            //_view.VietnameseContent = model.VietnameseContent;
            //_view.AvatarPath = model.AvatarPath;

            //if (model.Tags != null)
            //{
            //    _view.Tags = model.Tags;
            //}
            //else
            //{
            //    _view.Tags = model.Title.ToWords().First();
            //}

            _view.Files = _manager.GetFiles(model.Id).ToList();
            var _comment = new CommentModel();
            _comment.ArticleId = model.Id;
            _comment.Title = "Reply on [" + model.Title + "]";
            if(_comment.ArticleId.HasValue)
            {
                _view.Article = _manager.GetById(_comment.ArticleId.Value);
            }
                                    
            return _view;
        }
        public List<CommentView> ToViews(List<CommentModel> models)
        {
            List<CommentView> _views = new List<CommentView>();
            foreach (var model in models)
            {
                _views.Add(ToView(model));
            }
            return _views.ToList();
        }
    }
}