using Mvc5.CafeT.vn.Models;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class UrlsController : BaseController
    {
        public UrlsController(IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
        }

        //http://blog.instance-factory.com/?p=1147
        public ActionResult Index(int? page, string searchString)
        {
            List<UrlModel> _models = new List<UrlModel>();

            if (!String.IsNullOrEmpty(searchString))
            {
                _models = _models.Where(s => s.Url.Contains(searchString) 
                || (s.Name != null && s.Name.Contains(searchString)))
                .OrderByDescending(t=>t.CreatedDate).ToList();
            }

            if(Request.IsAjaxRequest())
            {
                return (PartialView( _models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
            }
            return (View(_models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
        }

        // GET: Urls/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UrlModel _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            if (_model == null)
            {
                return HttpNotFound();
            }

            return View(_model);
        }

        // GET: Urls/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            UrlModel _model = new UrlModel();
            return View(_model);
        }

        // POST: Urls/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(UrlModel model)
        {
            _unitOfWorkAsync.Repository<UrlModel>().Insert(model);
            try
            {
                // TODO: Add insert logic here
                model.LastUpdatedBy = User.Identity.Name;
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: Urls/Edit/5
        public ActionResult Edit(Guid id)
        {
            var _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            return View(_model);
        }

        // POST: Urls/Edit/5
        [HttpPost]
        public ActionResult Edit(UrlModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            model.LastUpdatedBy = User.Identity.Name;
            _unitOfWorkAsync.Repository<UrlModel>().Update(model);
            try
            {
                // TODO: Add insert logic here
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: Urls/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _model = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            if (_model == null)
            {
                return HttpNotFound();
            }
            return View(_model);
        }

        // POST: Urls/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
