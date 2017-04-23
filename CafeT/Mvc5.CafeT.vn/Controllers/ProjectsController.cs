using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ProjectsController : BaseController
    {

        public ProjectsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        // GET: Articles
        public ActionResult Index()
        {
            var _objects = _unitOfWorkAsync.Repository<ProjectModel>().Query().Select();
            return View(_objects.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(Guid id,string seoName)
        {
            var _object = _projectManager.GetById(id);
            _object.Articles = _projectManager.GetArticles(id).ToList();
            _object.Files = _projectManager.GetFiles(id).ToList();

            // Redirect to proper name
            if (seoName != Helpers.Extensions.SeoName(_object.Name))
                return RedirectToActionPermanent("Details", new { id = id, seoName = Helpers.Extensions.SeoName(_object.Name) });
            return View(_object);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddFile(Guid? id, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/Images"), fileName);

                try
                {
                    file.SaveAs(path);
                    FileModel _file = new FileModel(path);
                    _file.CreatedBy = User.Identity.Name;
                    _file.ProjectId = id.Value;
                    _projectManager.AddFile(id.Value, _file);
                    _unitOfWorkAsync.SaveChanges();
                    ViewBag.Message = "Upload successful";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Upload failed";
                    return RedirectToAction("Uploads");
                }
            }

            ViewBag.Message = "Upload failed";
            return RedirectToAction("Uploads");

        }

        // GET: Articles/Create
        [Authorize]
        public ActionResult AddArticle(Guid projectId)
        {
            ArticleModel _article = new ArticleModel();
            _article.ProjectId = projectId;
            if(Request.IsAjaxRequest())
            {
                return PartialView("_AddArticle",_article);
            }
            return View("_AddArticle", _article);
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult AddArticle(ArticleModel article)
        {
            try
            {
                article.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<ArticleModel>().Insert(article);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("_Errors", ex.Message);
            }
        }

        // GET: Articles/Create
        [Authorize]
        public ActionResult Create()
        {
            ProjectModel model = new ProjectModel("Create");
            return View(model);
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(ProjectModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<ProjectModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articles/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _project = _projectManager.GetById(id);
            return View(_project);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(ProjectModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedBy = User.Identity.Name;
                if(_projectManager.Update(model))
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Articles/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: Articles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(ProjectModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (_projectManager.Delete(model))
                {
                    return RedirectToAction("Index");
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
