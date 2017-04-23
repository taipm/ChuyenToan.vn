using System;
using System.Linq;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;
using PagedList;
using CafeT.Text;

namespace Mvc5.CafeT.vn.Controllers
{
    public class QuestionsController : BaseController
    {
        public QuestionsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        // GET: Questions
        public ActionResult Index(int? page, string searchString)
        {
            var _objects = _unitOfWorkAsync.Repository<QuestionModel>().Query().Select()
                .OrderByDescending(t => t.CreatedDate).ToList(); 

            if (!searchString.IsNullOrEmptyOrWhiteSpace())
            {
                _objects = _objects.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                            || (s.Content != null && s.Content.ToLower().Contains(searchString.ToLower()))).ToList();
            }

            ViewBag.NoAnswers = _questionManager.GetAllNoAnswers();
            ViewBag.TopAnswers = _questionManager.GetTopAnswers().Take(5);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Questions", _objects.ToPagedList(pageNumber: page ?? 1, pageSize: 10));
            }
            return View(_objects.ToPagedList(pageNumber: page ?? 1, pageSize: 10));
        }
        public ActionResult GetAllByLevel(int level)
        {
            var _objects = _questionManager.GetAllByLevel(level);
            if(Request.IsAjaxRequest())
            {
                return PartialView("_Questions", _objects);
            }
            return View("_Questions", _objects);
        }
        // GET: Questions/Details/5
        public ActionResult Details(Guid id)
        {
            var _object = _questionManager.GetById(id);
           
            if (_object != null && _object.EstimationTime != null)
            {
                _object.Answers = _questionManager.GetAnswers(id).ToList();
                Session.Clear();
                if (Session["EndDate"] == null)
                {
                    Session["EndDate"] = DateTime.Now.AddMinutes(_object.EstimationTime.Value).ToString("dd-MM-yyyy h:mm:ss tt");
                }

                ViewBag.EndDate = Session["EndDate"];
                ViewBag.AnswerCreate = new AnswerModel() { QuestionId = id };

                if(_object.JobId != null && _object.JobId.HasValue)
                {
                    ViewBag.Job = _jobManager.GetById(_object.JobId.Value);
                }
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Articles/Create
        [HttpGet]
        [Authorize]
        public ActionResult GetAnswers(Guid questionId)
        {
            var _answers = _questionManager.GetAnswers(questionId);
            if (_answers != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Answers", _answers);
                }
                return View("_Answers", _answers);
            }
            return HttpNotFound();
        }

        // POST: Articles/Create
        [HttpGet]
        [Authorize]
        public ActionResult AddAnswer(Guid id)
        {
            var _question = _questionManager.GetById(id);
            if (_question == null)
                return View("_HtmlString", "Question is null. Can't add answer");
            else
            {
                if(_question.CreatedBy != User.Identity.Name)
                {
                    if(_question.IsVerified)
                    {
                        AnswerModel _answer = new AnswerModel();
                        _answer.CreatedBy = User.Identity.Name;
                        _answer.QuestionId = id;

                        ViewBag.Question = _question;

                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("_UserCreateAnswer", _answer);
                        }
                        return View("_UserCreateAnswer", _answer);
                    }
                    else
                    {
                        return View("_HtmlString", "Question is not verified. Can't add answer");
                    }
                }
                else
                {
                    AnswerModel _answer = new AnswerModel();
                    _answer.CreatedBy = User.Identity.Name;
                    _answer.QuestionId = id;

                    ViewBag.Question = _question;

                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_UserCreateAnswer", _answer);
                    }
                    return View("_UserCreateAnswer", _answer);
                }
                
            }
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreateAnswer(AnswerModel model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    var _objects = _questionManager.GetAnswers(model.QuestionId.Value);
                    return PartialView("_Answers", _objects);
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("_Errors", ex.Message);
            }
        }

        // POST: Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreateAnswer(AnswerModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.CreatedBy = User.Identity.Name;
                _unitOfWorkAsync.Repository<AnswerModel>().Insert(model);
                _unitOfWorkAsync.SaveChanges();
                if(Request.IsAjaxRequest())
                {
                    var _objects = _questionManager.GetAnswers(model.Id);
                    return PartialView("_Answers", _objects);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Questions/Create
        [Authorize]
        public ActionResult Create()
        {
            QuestionModel _object = new QuestionModel();
            return View(_object);
        }

        // POST: Questions/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionModel question)
        {
            try
            {
                // TODO: Add insert logic here
                question.CreatedBy = User.Identity.Name;
                if(_questionManager.Insert(question))
                {
                    return RedirectToAction("Index");
                }
                return View("CanNotCreate");
            }
            catch
            {
                return View();
            }
        }

        // GET: Questions/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<QuestionModel>().Find(id);
            if (_object != null)
            {
                if (_object.CreatedBy == User.Identity.Name)
                {
                    if (!_object.Authors.IsNullOrEmptyOrWhiteSpace())
                    {
                        _object.Authors = _object.CreatedBy;
                    }
                    return View(_object);
                }
                else
                {
                    return View("_EditNotification");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionModel edit)
        {
            // TODO: Add update logic here
            edit.LastUpdatedDate = DateTime.Now;
            edit.LastUpdatedBy = User.Identity.Name;
            _unitOfWorkAsync.Repository<QuestionModel>().Update(edit);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                if(Request.IsAjaxRequest())
                {
                    return PartialView("_UpdatedMessage");
                }
                return RedirectToAction("Details", new { id = edit.Id });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Questions/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _questionManager.GetById(id);
            if (_object != null)
            {
                if(_object.CreatedBy == User.Identity.Name)
                {
                    return View("Delete", _object);
                }
                else
                {
                    return View("_DeleteNotification");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Questions/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(QuestionModel model)
        {
            if (_questionManager.Delete(model.Id))
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_DeletedMessage");
                }
                return RedirectToAction("Index");
            }
            return View("_HtmlString", "Can not Delete this question");
        }
    }
}
