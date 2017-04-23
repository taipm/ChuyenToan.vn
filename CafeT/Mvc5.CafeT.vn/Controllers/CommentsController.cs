using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class CommentsController : BaseController
    {
        public CommentsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
        [AllowAnonymous]
        public ActionResult TopComments()
        {
            var _objects = _commentManager.GetAll().OrderByDescending(m => m.CreatedDate).Take(3);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Comments-Box", _objects);
            }
            else
            {
                return View("_Comments-Box", _objects);
            }
        }        
        // GET: Articles
        public ActionResult Index()
        {
            IEnumerable<CommentModel> _objects = _commentManager.GetAll();
            IEnumerable<CommentView> _views = _mapper.ToViews(_objects.ToList());
            return View(_views);
        }

        // GET: Articles/Details/5
        public ActionResult Details(Guid id)
        {
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        // GET: Articles/Create

        public ActionResult Create()
        {
            CommentModel _article = new CommentModel();
            return View(_article);
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CommentModel comment)
        {
            try
            {
                // TODO: Add insert logic here
                if(_commentManager.Insert(comment))
                {
                    return RedirectToAction("Index");
                }
                return View("Errors");
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
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        // POST: Articles/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CommentModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = User.Identity.Name;
                if(_commentManager.Update(model))
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
            var _comment = _commentManager.GetById(id);
            return View(_comment);
        }

        // POST: Articles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(CommentModel model)
        {
            try
            {
                if (_commentManager.Update(model))
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
