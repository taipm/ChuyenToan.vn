using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class CuriculumsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CuriculumsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Curiculums
        public async Task<ActionResult> Index()
        {
            return View(await db.Curiculums.ToListAsync());
        }

        // GET: Curiculums/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = await db.Curiculums.FindAsync(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            return View(curiculumModel);
        }

        // GET: Curiculums/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curiculums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CuriculumModel curiculumModel)
        {
            if (ModelState.IsValid)
            {
                curiculumModel.CreatedBy = User.Identity.Name;
                db.Curiculums.Add(curiculumModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(curiculumModel);
        }

        // GET: Curiculums/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = await db.Curiculums.FindAsync(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            return View(curiculumModel);
        }

        // POST: Curiculums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CuriculumModel curiculumModel)
        {
            if (ModelState.IsValid)
            {
                curiculumModel.LastUpdatedDate = DateTime.Now;
                curiculumModel.LastUpdatedBy = User.Identity.Name;
                db.Entry(curiculumModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(curiculumModel);
        }

        // GET: Curiculums/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = await db.Curiculums.FindAsync(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            return View(curiculumModel);
        }

        // POST: Curiculums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CuriculumModel curiculumModel = await db.Curiculums.FindAsync(id);
            db.Curiculums.Remove(curiculumModel);
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
