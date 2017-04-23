using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class GenericController : Controller
    {
        // GET: Generic
        public ActionResult Index()
        {
            return View();
        }

        // GET: Generic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Generic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generic/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Generic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Generic/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: Generic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Generic/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
