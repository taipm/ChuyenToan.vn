using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class CompanyModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CompanyModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: CompanyModels
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: CompanyModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyModel companyModel = db.Companies.Find(id);
            if (companyModel == null)
            {
                return HttpNotFound();
            }
            //companyModel.Jobs = _unitOfWorkAsync.Repository<JobModel>().Query().Select()
            //    .Where(t => t.JobId == id.Value);
            return View(companyModel);
        }

        public ActionResult GetJobs(Guid companyId)
        {
            var _objects = _unitOfWorkAsync.Repository<JobModel>().Query().Select().Where(t => t.CompanyId == companyId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Jobs", _objects);
            }
            else
            {
                return View("_Jobs", _objects);
            }
        }

        [Authorize]
        public ActionResult AddJob(Guid companyId)
        {
            JobModel _object = new JobModel();
            _object.CompanyId = companyId;

            if(Request.IsAjaxRequest())
            {
                return PartialView("_AddJob", _object);
            }
            else
            {
                return View("_AddJob", _object);
            }
        }

        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddJob(JobModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = User.Identity.Name;
                db.Jobs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: CompanyModels/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel companyModel)
        {
            if (ModelState.IsValid)
            {
                companyModel.Id = Guid.NewGuid();
                companyModel.CreatedBy = User.Identity.Name;
                db.Companies.Add(companyModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyModel);
        }

        // GET: CompanyModels/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyModel companyModel = db.Companies.Find(id);
            if (companyModel == null)
            {
                return HttpNotFound();
            }
            return View(companyModel);
        }

        // POST: CompanyModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(CompanyModel companyModel)
        {
            if (ModelState.IsValid)
            {
                companyModel.LastUpdatedBy = User.Identity.Name;
                companyModel.LastUpdatedDate = DateTime.Now;
                db.Entry(companyModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyModel);
        }

        // GET: CompanyModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyModel companyModel = db.Companies.Find(id);
            if (companyModel == null)
            {
                return HttpNotFound();
            }
            return View(companyModel);
        }

        // POST: CompanyModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CompanyModel companyModel = db.Companies.Find(id);
            db.Companies.Remove(companyModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
