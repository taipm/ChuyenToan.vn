using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using System.IO;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Services;
using CafeT.Text;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ImageModelsController : BaseController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        protected readonly Mappers.Mappers _mapper;

        public ImageModelsController(
            IUnitOfWorkAsync unitOfWorkAsync,
            IArticleService service):base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _mapper = new Mappers.Mappers(_unitOfWorkAsync, service);
        }

        // GET: ImageModels
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/assets/img/"));
            var _images = _imageManager.GetAll().Select(x => { x.FullPath = x.FileName.AddBefore("../Uploads/Images/"); return x; });

            

            List<SliderModel> files = new List<SliderModel>();
            foreach(ImageModel _image in _images)
            {
                if(_image.FileName != null)
                {
                    string fileName = Path.GetFileName(_image.FileName);
                    files.Add(new SliderModel
                    {
                        Title = fileName.Split('.')[0].ToString(),
                        Src =  fileName.AddBefore("../Uploads/Images/"),
                        Description = _image.Description
                    });
                }
            }

            ViewBag.Sliders = files;

            return View(_images.ToList());
        }

        // GET: ImageModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageModel imageModel = db.Images.Find(id);
            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }
        [HttpPost]
        public ActionResult Upload(string description, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var _imageName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/Images"), _imageName);


                try
                {
                    file.SaveAs(path);
                    ImageModel _image = new ImageModel(path);
                    _image.Description = description;
                    _unitOfWorkAsync.Repository<ImageModel>().Insert(_image);
                    _unitOfWorkAsync.SaveChanges();
                    ViewBag.Message = "Upload successful";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Upload failed";
                    return RedirectToAction("Uploads");
                }
            }

            ViewBag.Message = "Upload failed";
            return RedirectToAction("Uploads");

        }
        // GET: ImageModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FullPath,CreatedDate,CreatedBy,LastUpdatedDate,ArticleId,FileId,CourseId,ProjectId")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                imageModel.Id = Guid.NewGuid();
                db.Images.Add(imageModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imageModel);
        }

        // GET: ImageModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageModel imageModel = db.Images.Find(id);
            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }

        // POST: ImageModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FullPath,CreatedDate,CreatedBy,LastUpdatedDate,ArticleId,FileId,CourseId,ProjectId")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imageModel);
        }

        // GET: ImageModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageModel imageModel = db.Images.Find(id);
            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }

        // POST: ImageModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ImageModel imageModel = db.Images.Find(id);
            db.Images.Remove(imageModel);
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
