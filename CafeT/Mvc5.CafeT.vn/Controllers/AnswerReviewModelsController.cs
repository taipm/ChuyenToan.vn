using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class AnswerReviewModelsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public AnswerReviewModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: AnswerReviewModels
        public async Task<ActionResult> Index()
        {
            var answerReviews = db.AnswerReviews.Include(a => a.Answer);
            return View(await answerReviews.ToListAsync());
        }

        // GET: AnswerReviewModels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerReviewModel answerReviewModel = await db.AnswerReviews.FindAsync(id);
            if (answerReviewModel == null)
            {
                return HttpNotFound();
            }
            return View(answerReviewModel);
        }

        // GET: AnswerReviewModels/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Name");
            return View();
        }

        // POST: AnswerReviewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(AnswerReviewModel answerReviewModel)
        {
            if (ModelState.IsValid)
            {
                answerReviewModel.Id = Guid.NewGuid();
                db.AnswerReviews.Add(answerReviewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Name", answerReviewModel.AnswerId);
            return View(answerReviewModel);
        }

        // GET: AnswerReviewModels/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerReviewModel answerReviewModel = await db.AnswerReviews.FindAsync(id);
            if (answerReviewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Name", answerReviewModel.AnswerId);
            return View(answerReviewModel);
        }

        // POST: AnswerReviewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(AnswerReviewModel answerReviewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answerReviewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Name", answerReviewModel.AnswerId);
            return View(answerReviewModel);
        }

        // GET: AnswerReviewModels/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerReviewModel answerReviewModel = await db.AnswerReviews.FindAsync(id);
            if (answerReviewModel == null)
            {
                return HttpNotFound();
            }
            return View(answerReviewModel);
        }

        // POST: AnswerReviewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            AnswerReviewModel answerReviewModel = await db.AnswerReviews.FindAsync(id);
            db.AnswerReviews.Remove(answerReviewModel);
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