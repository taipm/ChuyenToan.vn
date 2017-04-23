using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Mvc5.CafeT.vn.Managers;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ExamsController : BaseController
    {
        public ExamsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
        // GET: Exams
        public ActionResult Index()
        {
            var _objects = _unitOfWorkAsync.Repository<ExamModel>()
                .Query().Select();
            return View(_objects);
        }

        // GET: Exams/Details/5
        public ActionResult Details(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ExamModel>().Find(id);
            var _view = _mapper.ToView(_object);
            _view.Questions = _object.Questions;

            return View(_view);
        }
        // GET: Articles/Create
        [Authorize]
        public ActionResult AddExitsQuestion(Guid examId, Guid questionId)
        {
            QuestionModel _model = _questionManager.GetById(questionId);
            if(_model != null && _model.IsVerified)
            {
                _model.ExamId = examId;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AddQuestion", _model);
                }
                return View("_AddQuestion", _model);
            }
            return View("_HtmlString", "This question is not valid to add for exam");
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult AddExitsQuestion(QuestionModel model)
        {
            try
            {
                model.CreatedBy = User.Identity.Name;
                // TODO: Add insert logic here
                _unitOfWorkAsync.Repository<QuestionModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("_Errors", ex.Message);
            }
        }
        // GET: Articles/Create
        [Authorize]
        public ActionResult AddQuestion(Guid examId)
        {
            QuestionModel _model = new QuestionModel();
            _model.ExamId = examId;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddQuestion", _model);
            }
            return View("_AddQuestion", _model);
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult AddQuestion(QuestionModel model)
        {
            try
            {
                model.CreatedBy = User.Identity.Name;
                // TODO: Add insert logic here
                _unitOfWorkAsync.Repository<QuestionModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("_Errors", ex.Message);
            }
        }
        // GET: Exams/Create
        public ActionResult Create()
        {
            ExamModel _model = new ExamModel();

            return View(_model);
        }

        // POST: Exams/Create
        [HttpPost]
        public ActionResult Create(ExamModel model)
        {
            try
            {
                // TODO: Add insert logic here
                _unitOfWorkAsync.Repository<ExamModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Exams/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ExamModel>().Find(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(ExamModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<ExamModel>().Update(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Details", new { id = model.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Exams/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ExamModel>().Find(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: FileModels/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(ExamModel model)
        {
            try
            {
                // TODO: Add delete logic here
                _unitOfWorkAsync.Repository<ExamModel>().Delete(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
