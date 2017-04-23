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
    public class MenuModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public MenuModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: MenuModels
        public async Task<ActionResult> Index()
        {
            return View(await db.Menus.ToListAsync());
        }

        // GET: MenuModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModel menuModel = await db.Menus.FindAsync(id);
            if (menuModel == null)
            {
                return HttpNotFound();
            }
            return View(menuModel);
        }

        // GET: MenuModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                menuModel.Id = Guid.NewGuid();
                db.Menus.Add(menuModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(menuModel);
        }

        // GET: MenuModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModel menuModel = await db.Menus.FindAsync(id);
            if (menuModel == null)
            {
                return HttpNotFound();
            }
            return View(menuModel);
        }

        // POST: MenuModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menuModel);
        }

        // GET: MenuModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModel menuModel = await db.Menus.FindAsync(id);
            if (menuModel == null)
            {
                return HttpNotFound();
            }
            return View(menuModel);
        }

        // POST: MenuModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            MenuModel menuModel = await db.Menus.FindAsync(id);
            db.Menus.Remove(menuModel);
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
