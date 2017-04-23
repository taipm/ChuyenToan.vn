using CafeT.Text;
using CafeT.SmartObjects;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CafeT.Html;
using System.Drawing;

namespace Mvc5.CafeT.vn.Managers
{
    public class ArticleManager:ObjectManager
    {
        private readonly IArticleService _articleService;

        public ArticleManager(IArticleService service, IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
            _articleService = service;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public IEnumerable<ArticleModel> GetAll()
        {
            var _articles = _articleService.GetAll().OrderByDescending(t=>t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetAllPublished()
        {
            var _articles = _articleService.GetAll().Where(t => t.IsPublished)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }
        public IEnumerable<ArticleModel> GetAllUnPublished()
        {
            var _articles = _articleService.GetAll().Where(t => !t.IsPublished)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }
        public IEnumerable<ArticleModel> GetAllPublished(string createBy)
        {
            var _articles = _articleService.GetAll().Where(t => t.IsPublished && t.CreatedBy == createBy)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetTopViews(int? n)
        {
            var _articles = GetAllPublished();
            if (n.HasValue && n.Value >= n)
                return _articles.OrderByDescending(t=>t.CountViews).Take(n.Value);
            return _articles;
        }

        public IEnumerable<ArticleModel> Related(Guid id)
        {
            var _article = GetById(id);
            var _articles = GetAllPublished().Where(t=>t.Id != id);
            var _related = _articles.Where(t => !t.Tags.IsNullOrEmptyOrWhiteSpace() 
                                            && !_article.Tags.IsNullOrEmptyOrWhiteSpace()
                                            && t.Tags.IntersectWords(_article.Tags).Count() > 1);
            return _related;
        }

        
        public IEnumerable<ArticleModel> GetAllDrafted()
        {
            var _articles = _articleService.GetAll().Where(t => !t.IsPublished)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<ArticleModel> GetAllDrafted(string createBy)
        {
            var _articles = _articleService.GetAll().Where(t => !t.IsPublished && t.CreatedBy == createBy)
                .OrderByDescending(t => t.CreatedDate);
            return _articles;
        }

        public IEnumerable<string> GetAllTags()
        {
            List<string> _strs = new List<string>();
            var _tags = _articleService.GetAllTags();
            foreach(string _tag in _tags)
            {
                if(!_tag.IsNullOrEmptyOrWhiteSpace())
                {
                    _strs.AddRange(_tag.ToSmartText().Words);
                }
                
            }
            return _strs.Distinct();
        }

        public ArticleModel GetById(Guid id)
        {
            var _article = _articleService.GetById(id);            
            return _article;
        }

        public List<string> Process(Guid id)
        {
            List<string> _objects = new List<string>();
            var _article = _articleService.GetById(id);
            if(!_article.Content.IsNullOrEmptyOrWhiteSpace())
            {
                string[] _commands = _article.Content.ExtractAllBetween("@{", "}@");
                if (_commands != null && _commands.Count() > 0)
                {
                    foreach (string _command in _commands)
                    {
                        _objects = this.GetAll().Select(t => t.Title).ToList();
                    }
                }
            }
            
            return _objects;
        }

        //public string GetDataUrl(string imgFile)
        //{
        //    return "<img src=\"data:image/"
        //                + Path.GetExtension(imgFile).Replace(".", "")
        //                + ";base64,"
        //                + Convert.ToBase64String(File.ReadAllBytes(imgFile)) + "\" />";
        //}

        public System.Drawing.Image Base64ToImage(string base64String)
        {

            //var _base64Data = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            string _data = base64String.DeleteBeginTo("base64,").Replace("base64,", string.Empty).DeleteEndTo("\" />");
            var binData = Convert.FromBase64String(_data);
            Bitmap _image;
            using (var stream = new MemoryStream(binData))
            {
                _image = new Bitmap(stream);
            }
            return _image;
        }

        public string[] GetInnerImages(Guid id)
        {
            List<string> images = new List<string>();
            var _article = GetById(id);
            if(_article != null)
            {
                List<string> _images = new List<string>();
                _images = _article.Content.ExtractImages().ToList();
                if (_images != null && _images.Count() > 0)
                {
                    int i = 0;
                    foreach (var _img in _images)
                    {
                        if (_img.Contains("data:image"))
                        {
                            string _imagePath = @"C:\TmpC#\" + id.ToString() + i.ToString();

                            Image _image = Base64ToImage(_img);
                            _image = ResizeImage(_image, new Size() { Width = 100, Height = 150 });
                            _image.Save(_imagePath+".png");
                            images.Add(_imagePath + ".png");
                        }
                        i = i + 1;
                    }
                }
                return images.ToArray();
            }
            return null;
        }

        public Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public void ProcessUrls(Guid id)
        {
            var _article = _articleService.GetById(id);
            var _urls = _article.Content.ExtractUrlsWithoutHref();
            if(_urls != null && _urls.Count() >0)
            {
                foreach(string _url in _urls)
                {
                    if(_url.IsUrl())
                    {
                        UrlModel _object = new UrlModel(_url);
                        new UrlManager(_unitOfWorkAsync).Insert(_object);
                    }
                }
            }
        }

        public ArticleModel GetToView(Guid id, string userView)
        {
            var _article = _articleService.GetById(id);
            _article.CountViews = _article.CountViews + 1;
            _article.LastViewBy = userView;
            _article.LastViewAt = DateTime.Now;

            if (_article.Points <= 0) _article.Points = 0;

            //if(_article.CountViews >= 100)
            //{                
            //    _article.Points = _article.Points + 1;
            //}

            //if (_article.Comments != null && _article.Comments.Count() > 0)
            //{
            //    _article.Points = _article.Points + 1;
            //}

            //if (_article.Content != null && _article.Content.ToSmartText().Words.Count() > 100)
            //{
            //    _article.Points = _article.Points + 1;
            //}

            //if (_article.Questions!= null && _article.Questions.Count() > 0)
            //{
            //    _article.Points = _article.Points + 1;
            //}
                        
            //if (_article.Content.ToSmartText().GoogleDriveUrls != null && _article.Content.ToSmartText().GoogleDriveUrls.Count()>0)
            //{
            //    if(_article.Tags != null && !_article.Tags.Contains("Google Drive"))
            //        _article.Tags += "; Google Drive";

            //    foreach(string url in _article.Content.ToSmartText().GoogleDriveUrls)
            //    {
            //        FileModel _file = new FileModel(url);
            //        _file.FileName = "(Must update) Google Drive";
            //        _file.ArticleId = _article.Id;
            //        _unitOfWorkAsync.RepositoryAsync<FileModel>().Insert(_file);
            //        _unitOfWorkAsync.SaveChanges();
            //    }
            //}

            Update(_article);

            _article.Comments = GetComments(id).ToList();
            _article.Questions = GetQuestions(id).ToList();

            return _article;
        }

        public bool Update(ArticleModel article)
        {
            _articleService.Update(article);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
            
        }
        public bool Delete(ArticleModel article)
        {
            _articleService.Delete(article);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }

        }

        public bool Insert(ArticleModel article)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleModel>().Insert(article);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                Notify(new string[] { "taipm.vn@gmail.com", "taipm.vn@outlook.com" });
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool AddQuestion(QuestionModel question)
        {
            _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Insert(question);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                Notify(new string[] { "taipm.vn@gmail.com", "taipm.vn@outlook.com" });
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        public IEnumerable<CommentModel> GetComments(Guid id)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<CommentModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId != null && c.ArticleId.HasValue && c.ArticleId.Value == id);

            return _comments.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetQuestions(Guid id)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId == id);
            return _comments.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetVerifiedQuestions(Guid id)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId == id && c.IsVerified);

            return _comments.AsEnumerable();
        }

        public IEnumerable<QuestionModel> GetUnVerifyQuestions(Guid id)
        {
            var _comments = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId == id && !c.IsVerified);

            return _comments.AsEnumerable();
        }

        public IEnumerable<FileModel> GetFiles(Guid id)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<FileModel>().Query()
                            .Select()
                            .Where(c => c.ArticleId != null && c.ArticleId.HasValue && c.ArticleId.Value == id);

            return _models.AsEnumerable();
        }

        public bool AddComment(Guid id, CommentModel comment)
        {
            if(comment.ArticleId != null && comment.ArticleId.HasValue && comment.ArticleId.Value == id)
            {
                _unitOfWorkAsync.RepositoryAsync<CommentModel>().Insert(comment);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    //Notify(new string[] { "taipm.vn@gmail.com", "taipm.vn@outlook.com" });
                    return true;
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
        public bool AddFile(Guid id, FileModel model)
        {
            if (model.ArticleId != null && model.ArticleId.HasValue && model.ArticleId.Value == id)
            {
                var _exitsFiles = GetFiles(id).Select(t=>t.FileName);
                if (_exitsFiles.Contains(model.FileName)) return false;
                _unitOfWorkAsync.RepositoryAsync<FileModel>().Insert(model);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return false;
        }
        public void Notify(string[] users)
        {
            foreach(string user in users)
            {
                Notify(user);   
            }
        }
        public void Notify(string user)
        {

        }

        #region Maintenance method
        public void Clean()
        {
            //if()
        }
        #endregion
    }
}