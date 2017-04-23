using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class IssuesController : BaseController
    {
        public IssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Issues
        public async Task<ActionResult> Index()
        {

            ViewBag.TodayTasks = _issueManager.GetAllInToday();
            ViewBag.YesterdayTasks = _issueManager.GetAllInYesterday();
            ViewBag.TomorrowTasks = _issueManager.GetAllInTomorrow();

            return View(_issueManager.GetAll());
        }

        // GET: Issues
        public async Task<ActionResult> GetAllOverdued()
        {
            var _objects = _issueManager.GetAllOverdued();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Issues", _objects);
            }
            return View("_Issues",_objects);
        }
        // GET: Issues
        public async Task<ActionResult> GetAllYesterday()
        {
            var _objects = _issueManager.GetAllInYesterday();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Issues", _objects);
            }
            return View("_Issues", _objects);
        }
        // GET: Issues
        public async Task<ActionResult> GetAllTomorrow()
        {
            var _objects = _issueManager.GetAllInTomorrow();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Issues", _objects);
            }
            return View("_Issues", _objects);
        }
        // GET: Issues
        public async Task<ActionResult> GetAllToday()
        {
            var _objects = _issueManager.GetAllInToday();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Issues", _objects);
            }
            return View("_Issues", _objects);
        }

        // GET: Issues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueModel issueModel = _issueManager.GetById(id.Value);
            if (issueModel == null)
            {
                return HttpNotFound();
            }
            return View(issueModel);
        }

        // GET: Issues/Create
        [Authorize]
        public ActionResult Create()
        {
            IssueModel _model = new IssueModel();
            _model.CreatedBy = User.Identity.Name;
            _model.Start = DateTime.Now;
            _model.End = DateTime.Now.AddDays(1);
            _model.ExcuteBy = User.Identity.Name;
            _model.VerifyBy = User.Identity.Name;
            return View(_model);
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(IssueModel issueModel)
        {
            if (ModelState.IsValid)
            {
                if(_issueManager.Insert(issueModel))
                {
                    return RedirectToAction("Index");
                }
                return View(issueModel);
            }

            return View(issueModel);
        }

        // GET: Issues/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueModel issueModel = _issueManager.GetById(id.Value);
            if (issueModel == null)
            {
                return HttpNotFound();
            }
            return View(issueModel);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(IssueModel issueModel)
        {
            if (ModelState.IsValid)
            {
                issueModel.LastUpdatedDate = DateTime.Now;
                issueModel.LastUpdatedBy = User.Identity.Name;
                try
                {
                    if(_issueManager.Update(issueModel))
                    {
                        return RedirectToAction("Details", new { id = issueModel.Id });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(issueModel);
        }

        // GET: Issues/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueModel issueModel = _issueManager.GetById(id.Value);
            if (issueModel == null)
            {
                return HttpNotFound();
            }
            return View(issueModel);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            IssueModel issueModel = _issueManager.GetById(id);
            if(_issueManager.Delete(issueModel))
            {
                return RedirectToAction("Index");
            }
            return View("Errors");
            
        }
    }
}
