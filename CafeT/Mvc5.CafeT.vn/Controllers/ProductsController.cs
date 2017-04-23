using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;
using System.Net;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ProductsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ProductsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Products
        public ActionResult Index()
        {
            var _views = _unitOfWorkAsync.Repository<ProductModel>()
                .Query().Select();
            return View(_views);
        }

        // GET: Products/Details/5
        public ActionResult Details(Guid id,string seoName)
        {
            var _object = _unitOfWorkAsync.Repository<ProductModel>().Find(id);
            // Redirect to proper name
            if (seoName != Helpers.Extensions.SeoName(_object.Name))
                return RedirectToActionPermanent("Details", new { id = id, seoName = Helpers.Extensions.SeoName(_object.Name) });
            return View(_object);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            ProductModel _model = new ProductModel();
            return View(_model);
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(ProductModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<ProductModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ProductModel>().Find(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProductModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedDate = DateTime.Now;
                _unitOfWorkAsync.Repository<ProductModel>().Update(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: JobModels/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel jobModel = db.Products.Find(id);
            if (jobModel == null)
            {
                return HttpNotFound();
            }
            return View(jobModel);
        }

        // POST: JobModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductModel model = db.Products.Find(id);
            db.Products.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
