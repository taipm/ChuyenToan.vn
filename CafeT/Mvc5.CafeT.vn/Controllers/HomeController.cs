using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Services;
using Mvc5.CafeT.vn.Models;
using System.IO;
using Mvc5.CafeT.vn.ModelViews;

namespace Mvc5.CafeT.vn.Controllers
{
    public class HomeController : BaseController
    {        
        public HomeController(IUnitOfWorkAsync unitOfWorkAsync, IArticleService articleService) : base(unitOfWorkAsync)
        {            
        }


        public ActionResult Index()
        {
            ViewBag.CountComments = _commentManager.GetAll().Count();
            ViewBag.CountArticles = _articleManager.GetAll().Count();
            ViewBag.CountJobs= _jobManager.GetAll().Count();
            ViewBag.CountFiles = _fileManager.GetAll().Count();

            var _allArticles = _articleManager.GetAllPublished();

            ViewBag.HotArticles = _mapper.ToViews(_allArticles.OrderByDescending(m => m.CountViews).Take(4).ToList());
            foreach(var item in (List<ArticleView>)ViewBag.HotArticles)
            {
                item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                if (item.Author != null && item.Author.AvatarPath != null)
                {
                    //var filepath = "~/Uploads/Images/" + item.Author.AvatarPath.Split(new string[] { @"/" }, StringSplitOptions.None).LastOrDefault();
                    //item.ImageAuthor = filepath.Replace("~", string.Empty);
                    item.ImageAuthor = item.Author.AvatarPath;
                    item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                }
                item.ImageModels = _imageManager.GetByArticleId(item.Id).ToList();
            }

            ViewBag.NewArticles = _mapper.ToViews(_allArticles.OrderByDescending(m => m.CreatedDate).Take(8).ToList());

            foreach (var item in (List<ArticleView>)ViewBag.NewArticles)
            {
                item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                if (item.Author != null && item.Author.AvatarPath != null)
                {
                    //var filepath = "~/Uploads/Images/" + item.Author.AvatarPath.Split(new string[] { @"/" }, StringSplitOptions.None).LastOrDefault();
                    //item.ImageAuthor = filepath.Replace("~", string.Empty);
                    item.ImageAuthor = item.Author.AvatarPath;
                    item.Author = UserManager.FindByNameAsync(item.CreatedBy).Result;
                }
                item.ImageModels = _imageManager.GetByArticleId(item.Id).ToList();
            }

            ViewBag.Files = _unitOfWorkAsync.Repository<FileModel>().Query().Select().Where(m => !string.IsNullOrWhiteSpace(m.Title))
                .OrderByDescending(t => t.CreatedDate).Take(4).ToList();

            return View();
        }

        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";            
            return View();
        }
    }
}