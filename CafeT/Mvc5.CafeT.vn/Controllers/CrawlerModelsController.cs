using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class CrawlerModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CrawlerModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: CrawlerModels
        public async Task<ActionResult> Index()
        {
            if(Request.IsAjaxRequest())
            {
                return PartialView("_Crawlers",await db.Crawlers.ToListAsync());
            }
            return View(await db.Crawlers.ToListAsync());
        }

        // GET: CrawlerModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlerModel crawlerModel = await db.Crawlers.FindAsync(id);
            if (crawlerModel == null)
            {
                return HttpNotFound();
            }
            if(Request.IsAjaxRequest())
            {
                return PartialView(crawlerModel);
            }
            return View(crawlerModel);
        }

        // GET: CrawlerModels/Create
        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }

        // POST: CrawlerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(CrawlerModel crawlerModel)
        {
            if (ModelState.IsValid)
            {
                crawlerModel.Id = Guid.NewGuid();
                db.Crawlers.Add(crawlerModel);
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index");
                }
                return RedirectToAction("Index");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(crawlerModel);
            }
            return View(crawlerModel);
        }

        // GET: CrawlerModels/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlerModel crawlerModel = await db.Crawlers.FindAsync(id);
            if (crawlerModel == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(crawlerModel);
            }
            return View(crawlerModel);
        }

        // POST: CrawlerModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(CrawlerModel crawlerModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crawlerModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index");
                }
                return RedirectToAction("Index");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(crawlerModel);
            }
            return View(crawlerModel);
        }

        // GET: CrawlerModels/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlerModel crawlerModel = await db.Crawlers.FindAsync(id);
            if (crawlerModel == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(crawlerModel);
            }
            return View(crawlerModel);
        }

        // POST: CrawlerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CrawlerModel crawlerModel = await db.Crawlers.FindAsync(id);
            db.Crawlers.Remove(crawlerModel);
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("Index");
            }
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
