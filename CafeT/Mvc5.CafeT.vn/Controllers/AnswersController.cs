using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;

namespace Mvc5.CafeT.vn.Controllers
{
    public class AnswersController : BaseController
    {
        public AnswersController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Answers
        public ActionResult Index()
        {
            var _objects = _answerManager.GetAll();
            return View(_objects);
        }

        // GET: Answers/Details/5
        public ActionResult Details(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            _object.Reviews = _answerManager.GetAnswerReviews(_object.Id);

            ViewBag.AnswersInQuestion = _questionManager.GetAnswers(id);

            if (_object.QuestionId != null && _object.QuestionId.HasValue)
            {
                var _question = _unitOfWorkAsync.Repository<QuestionModel>().Find(_object.QuestionId);
                ViewBag.Question = _question;
                return View(_object);
            }
            return HttpNotFound();
        }

        [Authorize]
        public ActionResult AddReview(Guid answerId)
        {
            AnswerReviewModel _object = new AnswerReviewModel();
            _object.AnswerId = answerId;
            _object.CreatedBy = User.Identity.Name;
            _object.ReviewBy = User.Identity.Name;
            _object.ReviewDate = DateTime.Now;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddReview", _object);
            }
            else
            {
                return View("_AddReview", _object);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddReview(AnswerReviewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.AnswerId != null && model.AnswerId.HasValue)
                {
                    _answerManager.AddReview(model.AnswerId.Value, model);
                    return RedirectToAction("Details", "Answers", new { id = model.AnswerId.Value });
                }
            }

            return View(model);
        }

        // GET: Answers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnswerModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Answers/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnswerModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = User.Identity.Name;
                if (_answerManager.Update(model))
                {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_Answer", model);
                    }
                    return RedirectToAction("Details", new { id = model.Id });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Answers/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Answers/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(AnswerModel model)
        {
            try
            {
                // TODO: Add update logic here
                if(_answerManager.Delete(model))
                {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_DeletedMessage");
                    }
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
