using CafeT.Html;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class WebPageController : BaseController
    {
        protected readonly Mappers.Mappers _mapper;
        protected readonly ArticleManager _manager;

        public WebPageController(IUnitOfWorkAsync unitOfWorkAsync, IArticleService service) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _manager = new ArticleManager(service, _unitOfWorkAsync);
            _mapper = new Mappers.Mappers(_unitOfWorkAsync, service);
        }

        // GET: WebPage
        public ActionResult Index()
        {
            var _pages = _unitOfWorkAsync.Repository<WebPageModel>()
                .Query().Select();
            return View("Index", _pages);
        }

        // GET: WebPage/Details/5
        public ActionResult Details(Guid id)
        {
            var _page = _unitOfWorkAsync.Repository<WebPageModel>().Find(id);
            if(!string.IsNullOrEmpty(_page.Url))
            {
                if(string.IsNullOrEmpty(_page.HtmlContent))
                {
                    _page.HtmlContent = _page.Page.LoadHtml(_page.Url);
                }
            }
            //_page.HtmlContent =  _page.HtmlContent.GetNodesById("contentdiv").FirstOrDefault().OuterHtml;
            return View(_page);
        }

        // GET: WebPage/Create
        [HttpGet]
        public ActionResult Create(string url)
        {
            WebPageModel _page = new WebPageModel(url);
            return View(_page);
        }

        // POST: WebPage/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(WebPageModel model)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    _unitOfWorkAsync.Repository<WebPageModel>().Insert(model);
                    _unitOfWorkAsync.SaveChanges();
                    return PartialView("Details", model);
                }
                return RedirectToAction("Details", model);
            }
            catch(Exception ex)
            {
                return View("Error",ex.Message);
            }
        }

        // GET: WebPage/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _page = _unitOfWorkAsync.Repository<WebPageModel>().Find(id);
            return View(_page);
        }

        // POST: WebPage/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WebPage/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _page = _unitOfWorkAsync.Repository<WebPageModel>().Find(id);
            return View(_page);
        }

        // POST: WebPage/Delete/5
        [HttpPost][Authorize]
        public ActionResult Delete(WebPageModel model)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    _unitOfWorkAsync.Repository<WebPageModel>().Delete(model);
                    _unitOfWorkAsync.SaveChanges();
                    return RedirectToAction("Index");
                }
                _unitOfWorkAsync.Repository<WebPageModel>().Delete(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
