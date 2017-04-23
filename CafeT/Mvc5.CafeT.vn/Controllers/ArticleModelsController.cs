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

namespace Mvc5.CafeT.vn.Controllers
{
    public class ArticleModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArticleModels
        public async Task<ActionResult> Index()
        {
            var articles = db.Articles.Include(a => a.Category);
            return View(await articles.ToListAsync());
        }

        // GET: ArticleModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = await db.Articles.FindAsync(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            return View(articleModel);
        }

        // GET: ArticleModels/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.ArticleCategories, "Id", "Name");
            return View();
        }

        // POST: ArticleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Summary,Content,Tags,IsPublished,IsDrafted,IsProtect,IsPublic,IsPrivate,CreatedDate,CreatedBy,LastUpdatedDate,LastUpdatedBy,CountViews,LastViewAt,LastViewBy,EnglishContent,VietnameseContent,Points,CourseId,ProjectId,CompanyId,JobId,CategoryId,Followers,FromUrl")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                articleModel.Id = Guid.NewGuid();
                db.Articles.Add(articleModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.ArticleCategories, "Id", "Name", articleModel.CategoryId);
            return View(articleModel);
        }

        // GET: ArticleModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = await db.Articles.FindAsync(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.ArticleCategories, "Id", "Name", articleModel.CategoryId);
            return View(articleModel);
        }

        // POST: ArticleModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Summary,Content,Tags,IsPublished,IsDrafted,IsProtect,IsPublic,IsPrivate,CreatedDate,CreatedBy,LastUpdatedDate,LastUpdatedBy,CountViews,LastViewAt,LastViewBy,EnglishContent,VietnameseContent,Points,CourseId,ProjectId,CompanyId,JobId,CategoryId,Followers,FromUrl")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.ArticleCategories, "Id", "Name", articleModel.CategoryId);
            return View(articleModel);
        }

        // GET: ArticleModels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = await db.Articles.FindAsync(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            return View(articleModel);
        }

        // POST: ArticleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ArticleModel articleModel = await db.Articles.FindAsync(id);
            db.Articles.Remove(articleModel);
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
