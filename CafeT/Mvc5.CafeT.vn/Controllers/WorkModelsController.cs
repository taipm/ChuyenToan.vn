using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System.Linq;

namespace Mvc5.CafeT.vn.Controllers
{
    public class WorkModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public WorkModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: WorkModels
        public async Task<ActionResult> Index()
        {
            ViewBag.HotJobs = _jobManager.GetHotJobs(10);

            return View(await db.Works.ToListAsync());
        }

        // GET: WorkModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkModel workModel = await db.Works.FindAsync(id);
            if (workModel == null)
            {
                return HttpNotFound();
            }
            CuriculumModel _curiculum = _unitOfWorkAsync.Repository<CuriculumModel>().Query()
                .Select().Where(t => t.UserId == workModel.UserId).FirstOrDefault();

            ViewBag.Curiculum = _curiculum;

            return View(workModel);
        }

        // GET: WorkModels/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(WorkModel workModel)
        {
            if (ModelState.IsValid)
            {
                workModel.CreatedBy = User.Identity.Name;
                db.Works.Add(workModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(workModel);
        }

        // GET: WorkModels/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkModel workModel = await db.Works.FindAsync(id);
            if (workModel == null)
            {
                return HttpNotFound();
            }

            
            return View(workModel);
        }

        // POST: WorkModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(WorkModel workModel)
        {
            if (ModelState.IsValid)
            {
                workModel.LastUpdatedDate = DateTime.Now;
                workModel.LastUpdatedBy = User.Identity.Name;
                db.Entry(workModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(workModel);
        }

        // GET: WorkModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkModel workModel = await db.Works.FindAsync(id);
            if (workModel == null)
            {
                return HttpNotFound();
            }
            return View(workModel);
        }

        // POST: WorkModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WorkModel workModel = await db.Works.FindAsync(id);
            db.Works.Remove(workModel);
            await db.SaveChangesAsync();
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
