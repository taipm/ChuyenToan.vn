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
    public class MenuItemModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public MenuItemModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: MenuItemModels
        public async Task<ActionResult> Index()
        {
            return View(await db.MenuItems.ToListAsync());
        }

        // GET: MenuItemModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItemModel menuItemModel = await db.MenuItems.FindAsync(id);
            if (menuItemModel == null)
            {
                return HttpNotFound();
            }
            return View(menuItemModel);
        }

        // GET: MenuItemModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItemModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuItemModel menuItemModel)
        {
            if (ModelState.IsValid)
            {
                menuItemModel.Id = Guid.NewGuid();
                db.MenuItems.Add(menuItemModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(menuItemModel);
        }

        // GET: MenuItemModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItemModel menuItemModel = await db.MenuItems.FindAsync(id);
            if (menuItemModel == null)
            {
                return HttpNotFound();
            }
            return View(menuItemModel);
        }

        // POST: MenuItemModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MenuItemModel menuItemModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuItemModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menuItemModel);
        }

        // GET: MenuItemModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItemModel menuItemModel = await db.MenuItems.FindAsync(id);
            if (menuItemModel == null)
            {
                return HttpNotFound();
            }
            return View(menuItemModel);
        }

        // POST: MenuItemModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            MenuItemModel menuItemModel = await db.MenuItems.FindAsync(id);
            db.MenuItems.Remove(menuItemModel);
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
