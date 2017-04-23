using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class JobModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JobModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: JobModels
        public ActionResult Index()
        {
            var _messages = db.Messages;
            if(_messages != null && _messages.Count() > 0)
            {
                ViewBag.JobHelper = _messages.FirstOrDefault().Message;
            }

            var _objects = _jobManager.GetAll();

            foreach(var _object in _objects)
            {
                if(_object.CompanyId.HasValue)
                {
                    _object.Company = _jobManager.GetCompany(_object.CompanyId.Value);
                }
            }

            ViewBag.HotJobs = _objects.OrderByDescending(t => t.CountViews);

            ViewBag.NewQuestions = _unitOfWorkAsync.Repository<QuestionModel>().Query().Select()
                .OrderByDescending(t => t.CreatedDate);

            var _companies = _unitOfWorkAsync.Repository<CompanyModel>().Query().Select()
                .OrderByDescending(t => t.CreatedDate);

            ViewBag.Companies = _companies.Take(10);
            ViewBag.CountOfCompanies = _companies.Count();

            return View(_objects.ToList());
        }

        // GET: JobModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobModel jobModel = db.Jobs.Find(id);

            if (jobModel == null)
            {
                return HttpNotFound();
            }

            if(jobModel.CompanyId != null && jobModel.CompanyId.HasValue)
            {
                ViewBag.Company = _unitOfWorkAsync.Repository<CompanyModel>().Find(jobModel.CompanyId.Value);
            }

            jobModel.Questions = _unitOfWorkAsync.Repository<QuestionModel>().Query().Select()
                .Where(t => t.JobId == id.Value);
            return View(jobModel);
        }

        [AllowAnonymous]
        public ActionResult TopJobs()
        {
            var _objects = _jobManager.GetAll().OrderByDescending(m => m.SalaryInMoth).Take(4);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Jobs-Box", _objects);
            }
            else
            {
                return View("_Jobs-Box", _objects);
            }
        }

        [Authorize]
        public ActionResult AddQuestion(Guid jobId)
        {
            QuestionModel _object = new QuestionModel();
            _object.JobId = jobId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddQuestionForJob", _object);
            }
            else
            {
                return View("_AddQuestionForJob", _object);
            }
        }

        public ActionResult GetQuestions(Guid jobId)
        {
            var _objects = _unitOfWorkAsync.Repository<QuestionModel>().Query().Select()
                .Where(t => t.JobId == jobId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_QuestionsOfJob", _objects);
            }
            else
            {
                return View("_QuestionsOfJob", _objects);
            }
        }
        // GET: JobModels/Create
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            var _companies = _unitOfWorkAsync.Repository<CompanyModel>().Query().Select();
            List<SelectListItem> _companyList = new List<SelectListItem>();
            CompanyModel _defaultCompany = new CompanyModel();
            foreach (var item in _companies)
            {
                _companyList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item == _defaultCompany ? true : false)
                });
            }
            var selectList = new SelectList(_companyList, "Value", "Text");
            ViewBag.Companies = selectList;

            return View();
        }

        // POST: JobModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobModel jobModel)
        {
            if (ModelState.IsValid)
            {
                jobModel.Id = Guid.NewGuid();
                jobModel.CreatedBy = User.Identity.Name;
                if(_jobManager.Insert(jobModel))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(jobModel);
        }

        // GET: JobModels/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobModel jobModel = db.Jobs.Find(id);
            if (jobModel == null)
            {
                return HttpNotFound();
            }
            var _companies = _unitOfWorkAsync.Repository<CompanyModel>().Query().Select();
            List<SelectListItem> _companyList = new List<SelectListItem>();

            CompanyModel _defaultCompany = new CompanyModel();

            if(jobModel.CompanyId != null)
            {
                _defaultCompany = _jobManager.GetCompany(jobModel.CompanyId.Value);
            }

            foreach (var item in _companies)
            {
                _companyList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item == _defaultCompany ? true : false)
                });
            }

            var selectList = new SelectList(_companyList, "Value", "Text");

            ViewBag.Companies = selectList;

            return View(jobModel);
        }

        // POST: JobModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobModel jobModel, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                jobModel.LastUpdatedBy = User.Identity.Name;
                jobModel.LastUpdatedDate = DateTime.Now;
                if(_jobManager.Update(jobModel))
                {
                    return RedirectToAction("Details", new { id = jobModel.Id });
                }
            }
            return View(jobModel);
        }

        // GET: JobModels/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobModel jobModel = db.Jobs.Find(id);
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
            JobModel jobModel = db.Jobs.Find(id);
            if(_jobManager.Delete(jobModel))
            {
                return RedirectToAction("Index");
            }
            return View(jobModel);
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
