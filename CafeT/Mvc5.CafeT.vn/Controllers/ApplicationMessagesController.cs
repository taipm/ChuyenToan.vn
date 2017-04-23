using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ApplicationMessagesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationMessagesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: ApplicationMessages
        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }

        // GET: ApplicationMessages/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationMessage applicationMessage = db.Messages.Find(id);
            if (applicationMessage == null)
            {
                return HttpNotFound();
            }
            return View(applicationMessage);
        }

        // GET: ApplicationMessages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ApplicationMessage applicationMessage)
        {
            if (ModelState.IsValid)
            {
                applicationMessage.Id = Guid.NewGuid();
                applicationMessage.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<ApplicationMessage>().Insert(applicationMessage);
                _unitOfWorkAsync.SaveChanges();

                //db.Messages.Add(applicationMessage);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationMessage);
        }

        // GET: ApplicationMessages/Edit/5
        [Authorize]

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationMessage applicationMessage = db.Messages.Find(id);
            if (applicationMessage == null)
            {
                return HttpNotFound();
            }
            return View(applicationMessage);
        }

        // POST: ApplicationMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ApplicationMessage applicationMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationMessage);
        }

        // GET: ApplicationMessages/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationMessage applicationMessage = db.Messages.Find(id);
            if (applicationMessage == null)
            {
                return HttpNotFound();
            }
            return View(applicationMessage);
        }

        // POST: ApplicationMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ApplicationMessage applicationMessage = db.Messages.Find(id);
            db.Messages.Remove(applicationMessage);
            db.SaveChanges();
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
