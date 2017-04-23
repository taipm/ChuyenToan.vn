using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Linq;
using System.Web.Mvc;
using Mvc5.CafeT.vn.ModelViews;
using CafeT.Text;
using PagedList;
using System.Collections.Generic;
using Mvc5.CafeT.vn.Services;
using CafeT.SmartObjects;
using EyeOpen.Imaging;
using System.IO;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ArticlesController : BaseController
    {
        protected const int NUMBER_GET_ITEMS = 5;
        protected const int PAGE_ITEMS = 10;

        public ArticlesController(IUnitOfWorkAsync unitOfWorkAsync, IArticleService articleService) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;            
        }

        // GET: Articles
        public ActionResult Index(bool? all, int? page, string searchString)
        {
            var _objects = _articleManager.GetAllPublished().ToList();

            ViewBag.Categories = _articleCategoryManager.GetAll();

            ViewBag.TopViews = _mapper.ToViews(_articleManager.GetTopViews(NUMBER_GET_ITEMS).ToList());
            ViewBag.Tags = _articleManager.GetAllTags().ToList();

            var _files = _fileManager.GetFiles(null).OrderByDescending(c => c.CreatedDate);
            ViewBag.Files = _files.Take(NUMBER_GET_ITEMS);
            ViewBag.CountOfFiles = _files.Count();

            ViewBag.Projects = _projectManager.GetAll().OrderByDescending(c => c.CreatedDate).Take(NUMBER_GET_ITEMS);

            ViewBag.HotJobs = _jobManager.GetHotJobs(NUMBER_GET_ITEMS).OrderByDescending(t => t.CreatedBy);

            var _works = _workManager.GetAll();
            ViewBag.Works = _works.Take(NUMBER_GET_ITEMS);
            ViewBag.CountOfWorks = _works.Count();

            ViewBag.FeedUrls = _unitOfWorkAsync.Repository<UrlModel>().Query().Select()
                .OrderByDescending(t => t.CreatedDate).Take(PAGE_ITEMS);

            var _questions = _questionManager.GetAllVerified();

            ViewBag.Questions = _questions.Take(PAGE_ITEMS);
            ViewBag.CountOfQuestions = _questions.Count();

            ViewBag.Messages = _unitOfWorkAsync.Repository<ApplicationMessage>().Query().Select().FirstOrDefault();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }

            var _articleViews = _mapper.ToViews(_objects);
            ViewBag.Keyword = searchString;
            ViewBag.CountArticles = _articleViews.Count;

            foreach (var _articleView in _articleViews)
            {
                var _user = UserManager.FindByNameAsync(_articleView.CreatedBy);
                if(_user!= null)
                {
                    _articleView.Author = _user.Result;
                    _articleView.ImageAuthor = _user.Result.AvatarPath;                    
                }
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Index", _articleViews.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
        }

        public ActionResult GetAll(int? page, string searchString)
        {
            var _objects = _articleManager.GetAll().ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }

            var _views = _mapper.ToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        public ActionResult GetTopViews(int? page)
        {
            var _objects = _articleManager.GetAll().ToList();

            var _views = _mapper.ToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [Authorize]        
        public ActionResult GetAllUnPublished(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllUnPublished()
                .Where(t=>t.CreatedBy == User.Identity.Name)
                .ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }

            var _views = _mapper.ToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [Authorize]
        public ActionResult GetAllByUser(int? page, string searchString)
        {
            var _objects = _articleManager.GetAll().Where(t=>t.CreatedBy == User.Identity.Name).ToList();
            
            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }
            var _views = _mapper.ToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        public ActionResult GetAllPublished(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllPublished().ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }
            var _views = _mapper.ToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [Authorize]
        public ActionResult GetAllPublishedByUser(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllPublished().Where(t=>t.CreatedBy == User.Identity.Name).ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }
            var _views = _mapper.ToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [Authorize]
        public ActionResult GetAllDrafted(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllDrafted().ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }
            var _views = _mapper.ToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        [Authorize]
        public ActionResult GetAllDraftedByUser(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllDrafted().Where(t => t.CreatedBy == User.Identity.Name).ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Summary != null && s.Summary.ToLower().Contains(searchString.ToLower()))
                            || (s.Tags != null && s.Tags.ToLower().Contains(searchString.ToLower())))
                            .OrderByDescending(t => t.CreatedDate).ToList();
            }
            var _objectViews = _mapper.ToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _objectViews.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _objectViews.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        // GET: Articles
        public ActionResult Search(int? page, string searchString)
        {
            var _objects = _articleManager.GetAllPublished().ToList();

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.Contains(searchString)
                || (s.Title != null && s.Title.Contains(searchString)))
                .OrderByDescending(t => t.CreatedDate).ToList();
            }

            var _objectViews = _mapper.ToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", _objectViews.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
            }
            return View("_Articles", _objectViews.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        }

        // GET: Articles
        public ActionResult GetQuestions(Guid articleId)
        {
            var _objects = _articleManager.GetQuestions(articleId).ToList();
            if(Request.IsAjaxRequest())
            {
                return PartialView("_Questions", _objects);
            }
            return View("_Questions", _objects);
        }

        public ActionResult GetContent(Guid articleId)
        {
            ArticleModel _article = _articleManager.GetById(articleId);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_HtmlString", _article.Content);
            }
            return View("_HtmlString", _article.Content);
        }

        public ActionResult GetEnglishContent(Guid articleId)
        {
            ArticleModel _article = _articleManager.GetById(articleId);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_HtmlString", _article.EnglishContent);
            }
            return View("_HtmlString", _article.EnglishContent);
        }
        
        public ActionResult Details(Guid id, string seoName)
        {
            string _userView = string.Empty;
            if(User.Identity.IsAuthenticated)
            {
                _userView = User.Identity.Name;
            }
            ArticleModel _article = _articleManager.GetToView(id, _userView);
            var _view = _mapper.ToView(_article);            
            var _youTubeUrls = _view.Content.ToSmartText().YouTubeUrls;

            _view.Author = UserManager.FindByNameAsync(_view.CreatedBy).Result;

            if(_view.Author != null && _view.Author.AvatarPath != null)
            {                
                _view.ImageAuthor = _view.Author.AvatarPath;
                _view.Author = UserManager.FindByNameAsync(_view.CreatedBy).Result;
            }
            
            foreach (string _link in _youTubeUrls)
            {
                YouTubeView _youTube = new YouTubeView(_link);
                _view.Content = _view.Content.Replace(_link, _youTube.EmbedUrl);
            }

            ViewBag.Commands = _articleManager.Process(id);
            ViewBag.Image = _articleManager.GetInnerImages(id).FirstOrDefault();

            if (seoName != Helpers.Extensions.SeoName(_article.Title))
                return RedirectToActionPermanent("Details", new { id = id, seoName = Helpers.Extensions.SeoName(_article.Title) });

            ViewBag.RelatedArticles = _mapper.ToViews(_articleManager.Related(id).Take(4).ToList());

            foreach (var item in (List<ArticleView>)ViewBag.RelatedArticles)
            {
                item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                if (item.Author != null && item.Author.AvatarPath != null)
                {
                    var filepath = "~/Uploads/Images/" + item.Author.AvatarPath.Split(new string[] { @"/" }, StringSplitOptions.None).LastOrDefault();
                    item.ImageAuthor = filepath.Replace("~", string.Empty);
                    item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                }
                item.ImageModels = _imageManager.GetByArticleId(item.Id).ToList();
            }
            return View(_view);
        }
        
        
        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid articleId)
        {
            QuestionModel _model = new QuestionModel();

            _model.CreatedBy = User.Identity.Name;
            _model.ArticleId = articleId;
            if(Request.IsAjaxRequest())
            {
                return PartialView("_AddQuestion", _model);
            }
            else
            {
                return View("_AddQuestion", _model);
            }
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(QuestionModel model)
        {
            try
            {
                _articleManager.AddQuestion(model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Questions", _articleManager.GetComments(model.ArticleId.Value));
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
                }
            }
            catch
            {
                return View();
            }
        }
        // POST: Articles/Create
        [HttpGet]
        [Authorize]
        public ActionResult AddComment(Guid articleId)
        {
            CommentModel _model = new CommentModel();
            _model.CreatedBy = User.Identity.Name;
            _model.ArticleId = articleId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddComment", _model);
            }
            else
            {
                return View("_AddComment", _model);
            }
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentView view)
        {
            try
            {
                view.Id = Guid.NewGuid();
                view.CreatedBy = User.Identity.Name;
                view.CreatedDate = DateTime.Now;                

                CommentModel model = _mapper.ToModel(view);

                if (view.HasMeaning())
                {                    
                    _articleManager.AddComment(model.ArticleId.Value, model);
                }
                else
                {
                    string _error = "Comment không đủ ngữ nghĩa";
                    return PartialView("_Message", _error);

                    //HandleErrorInfo _error = new HandleErrorInfo()
                    //return PartialView("Error", _error);
                }

                if(Request.IsAjaxRequest())
                {
                    return PartialView("_comments", _articleManager.GetComments(model.ArticleId.Value));
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = model.ArticleId });
                }
            }
            catch(Exception ex)
            {
                HandleErrorInfo _error = new HandleErrorInfo(ex, "Articles", "AddComment");
                return View("Error", _error);
            }
        }
        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Follow(Guid articleId)
        {
            try
            {
                var _object = _articleManager.GetById(articleId);
                if(!_object.Followers.Contains(User.Identity.Name))
                {
                    _object.Followers = _object.Followers + User.Identity.Name + ";";
                    _object.LastUpdatedBy = User.Identity.Name;
                    _articleManager.Update(_object);
                }
                
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_HtmlString", _object.Followers);
                }
                else
                {
                    return RedirectToAction("Details", "Articles", new { id = articleId });
                }
            }
            catch
            {
                return View();
            }
        }

        public SelectList GetSelectListCategories(ArticleModel article)
        {
            List<SelectListItem> _categoryList = new List<SelectListItem>();
            var _categories = _unitOfWorkAsync.Repository<ArticleCategory>().Query().Select().OrderBy(m => m.Name);

            ArticleCategory _default = new ArticleCategory();

            if (article.CategoryId != null)
            {
                _default = _articleCategoryManager.GetById(article.CategoryId.Value);
            }

            foreach (var item in _categories)
            {
                _categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item == _default ? true : false)
                });
            }

            return new SelectList(_categoryList, "Value", "Text");
        }

        // GET: Articles/Create
        [Authorize]
        public ActionResult Create()
        {
            ArticleModel _article = new ArticleModel("Create" + DateTime.Today.DayOfYear.ToString());
            ViewBag.Categories = GetSelectListCategories(_article);

            return View(_article);
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleModel article)
        {
            ViewBag.Categories = GetSelectListCategories(article);

            try
            {
                article.CreatedBy = User.Identity.Name;
                article.IsPublished = false;
                if (_articleManager.Insert(article))
                {
                    _articleManager.ProcessUrls(article.Id);
                    return RedirectToAction("Index");
                }
                return View(article);
            }
            catch
            {
                return View(article);
            }
        }

        // GET: Articles/Edit/NUMBER_GET_ITEMS
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _model = _articleManager.GetById(id);
            if (_model != null)
            {
                var _categories = _unitOfWorkAsync.Repository<ArticleCategory>().Query().Select();
                List<SelectListItem> _categoryList = new List<SelectListItem>();

                ArticleCategory _default = new ArticleCategory();

                if (_model.CategoryId.HasValue)
                {
                    _default = _articleCategoryManager.GetById(_model.CategoryId.Value);
                }

                foreach (var item in _categories)
                {
                    _categoryList.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = (item == _default ? true : false)
                    });
                }

                var selectList = new SelectList(_categoryList, "Value", "Text");

                ViewBag.Categories = selectList;

                var _view = _mapper.ToView(_model);
                return View(_view);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Articles/Edit/NUMBER_GET_ITEMS
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]                
        
        public ActionResult Edit(Guid id, ArticleView view, FormCollection collection, string imageBase)
        {
            try
            {
                ArticleModel _article = _mapper.ToModel(view);
                _article.LastUpdatedDate = DateTime.Now;
                _article.IsPublished = false;
                _article.LastUpdatedBy = User.Identity.Name;
                if (!string.IsNullOrWhiteSpace(imageBase))
                {
                    var image = ImageUtility.Base64ToImage(imageBase.Replace("data:image/png;base64,", ""));
                    string folder = "/Profiles/Uploads/" + User.Identity.Name.Replace("@", "_") + "/Articles/";
                    string urlImage = folder + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".png";
                    if (!Directory.Exists(Server.MapPath("~" + folder)))
                        Directory.CreateDirectory(Server.MapPath("~" + folder));
                    image.Save(Server.MapPath("~" + urlImage));

                    _article.AvatarPath = urlImage;
                }
                if (_articleManager.Update(_article))
                {
                    return RedirectToAction("Details", new { id = _article.Id });
                }
                return View("_Error");
            }
            catch(Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        // POST: Articles/Edit/NUMBER_GET_ITEMS
        [HttpPost]
        [Authorize]
        public ActionResult ToPublish(Guid id)
        {
            try
            {
                var _model = _articleManager.GetById(id);
                _model.ToPublish();
                _articleManager.Update(_model);
                if(Request.IsAjaxRequest())
                {
                    return PartialView("_Published");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        // POST: Articles/Edit/NUMBER_GET_ITEMS
        [HttpPost]
        [Authorize]
        public ActionResult ToPublic(Guid id)
        {
            try
            {
                var _model = _articleManager.GetById(id);
                _model.ToPublish();
                _articleManager.Update(_model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Public");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToDraft(Guid id)
        {
            try
            {
                var _model = _articleManager.GetById(id);
                _model.ToDraft();
                _model.LastUpdatedBy = User.Identity.Name;
                _articleManager.Update(_model);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Drafted");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _model = _articleManager.GetById(id);
            if(_model != null)
            {
                return View(_model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ArticleModel model)
        {
            try
            {
                if(_articleManager.Delete(model))
                {
                    return RedirectToAction("Index","Articles");
                }
                return View();

            }
            catch
            {
                return View();
            }
        }
    }
}
