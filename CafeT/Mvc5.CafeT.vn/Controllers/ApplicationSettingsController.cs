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
    public class ApplicationSettingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationSettingsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: ApplicationSettings
        public async Task<ActionResult> Index()
        {
            return View(await db.Settings.ToListAsync());
        }

        // GET: ApplicationSettings/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = await db.Settings.FindAsync(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(ApplicationSetting applicationSetting)
        {
            if (ModelState.IsValid)
            {
                applicationSetting.Id = Guid.NewGuid();
                db.Settings.Add(applicationSetting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = await db.Settings.FindAsync(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // POST: ApplicationSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ApplicationSetting applicationSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationSetting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = await db.Settings.FindAsync(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // POST: ApplicationSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ApplicationSetting applicationSetting = await db.Settings.FindAsync(id);
            db.Settings.Remove(applicationSetting);
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
