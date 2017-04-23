using CafeT.Text;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class FileModelsController : BaseController
    {
        protected readonly ArticleManager _manager;
        protected readonly Mappers.Mappers _mapper;

        public FileModelsController(IUnitOfWorkAsync unitOfWorkAsync,
            IArticleService service) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _manager = new ArticleManager(service, _unitOfWorkAsync);
            _mapper = new Mappers.Mappers(_unitOfWorkAsync, service);
        }


        // generic file post method - use in MVC or WebAPI
        [HttpPost]
        public HttpResponseMessage UploadFileWithSplits()
        {
            string _baseFileName = string.Empty;
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using the original file.part name posted
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath("~/App_Data/Uploads");
                    Directory.CreateDirectory(UploadPath);
                    string path = Path.Combine(UploadPath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        // Once the file part is saved, see if we have enough to merge it
                        Shared.Utils UT = new Shared.Utils();
                        UT.MergeFile(path, out _baseFileName);
                        if(!_baseFileName.IsNullOrEmpty())
                        {
                            FileModel _file = new FileModel(_baseFileName);
                            _unitOfWorkAsync.Repository<FileModel>().Insert(_file);
                            _unitOfWorkAsync.SaveChanges();
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new HttpResponseMessage()
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Content = new StringContent("File uploaded.")
                        };
                        
                    }
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("File uploaded.")
            };
        }

        public ActionResult Upload()
        {
            return View();
        }
        

        public ActionResult Download()
        {
            string[] files = Directory.GetFiles(Server.MapPath("/Files"));
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            ViewBag.Files = files;
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), fileName);
        }

        // GET: FileModels
        public ActionResult Index()
        {
            var _models = _unitOfWorkAsync.Repository<FileModel>().Query().Select();
            return View(_models);
        }

        // GET: FileModels/Details/5
        public ActionResult Details(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            return View(_object);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/Images"), fileName);

                try
                {
                    file.SaveAs(path);
                    FileModel _file = new FileModel(path);
                    _unitOfWorkAsync.Repository<FileModel>().Insert(_file);
                    _unitOfWorkAsync.SaveChanges();
                    ViewBag.Message = "Upload successful";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Failed : "+ ex.Message;
                    return RedirectToAction("Uploads");
                }
            }

            ViewBag.Message = "Upload failed";
            return RedirectToAction("Uploads");

        }

        //// GET: FileModels/Create
        //public ActionResult Create()
        //{

        //    return View();
        //}

        //// POST: FileModels/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: FileModels/Edit/5
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            return View(_object);
        }

        // POST: FileModels/Edit/5
        [HttpPost]
        public ActionResult Edit(FileModel model)
        {
            try
            {
                // TODO: Add update logic here
                _unitOfWorkAsync.Repository<FileModel>().Update(model);
                _unitOfWorkAsync.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FileModels/Delete/5
        public ActionResult Delete(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            return View(_object);
        }

        // POST: FileModels/Delete/5
        [HttpPost]
        public ActionResult Delete(FileModel model)
        {
            try
            {
                // TODO: Add delete logic here
                _unitOfWorkAsync.Repository<FileModel>().Delete(model);
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
