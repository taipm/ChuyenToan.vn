using Microsoft.AspNet.Identity.Owin;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class BaseController : Controller
    {
        protected int PageSize = 10;
        protected string UploadFolder = "~/Profiles/Uploads/";

        protected IUnitOfWorkAsync _unitOfWorkAsync;
        protected IArticleService _articleService;
        protected ICommentService _commentService;

        protected readonly ArticleManager _articleManager;
        protected readonly ArticleCategoryManager _articleCategoryManager;
        protected readonly JobManager _jobManager;
        protected readonly CommentManager _commentManager;
        protected readonly WorkManager _workManager;
        protected readonly ProjectManager _projectManager;
        protected readonly FileManager _fileManager;
        protected readonly ImageManager _imageManager;
        protected readonly AnswerManager _answerManager;
        protected readonly UrlManager _urlManager;
        protected readonly QuestionManager _questionManager;
        protected readonly IssueManager _issueManager;
        protected readonly MenuManager _menuManager;
        protected readonly Mappers.Mappers _mapper;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public BaseController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;

            _projectManager = new ProjectManager(unitOfWorkAsync);
            _menuManager = new MenuManager(unitOfWorkAsync);
            _fileManager = new FileManager(unitOfWorkAsync);
            _jobManager = new JobManager(unitOfWorkAsync);
            _workManager = new WorkManager(unitOfWorkAsync);
            _imageManager = new ImageManager(unitOfWorkAsync);
            _answerManager = new AnswerManager(unitOfWorkAsync);
            _urlManager = new UrlManager(unitOfWorkAsync);
            _questionManager = new QuestionManager(unitOfWorkAsync);
            _issueManager = new IssueManager(unitOfWorkAsync);
            _articleCategoryManager = new ArticleCategoryManager(unitOfWorkAsync);
            
            _commentService = new CommentService(unitOfWorkAsync.RepositoryAsync<CommentModel>());
            _commentManager = new CommentManager(_commentService, unitOfWorkAsync);

            _articleService = new ArticleService(unitOfWorkAsync.RepositoryAsync<ArticleModel>());
            _articleManager = new ArticleManager(_articleService, unitOfWorkAsync);

            _mapper = new Mappers.Mappers(_unitOfWorkAsync, _articleService);

            var _menus = _menuManager.GetAll();
            ViewBag.Menus = _menus;

            var _message = _unitOfWorkAsync.Repository<ApplicationMessage>().Query().Select()
                .Where(t => t.Name == "Web_Url");

            if (_message.Count() > 0)
            {
                ViewBag.Web_Url = _message.FirstOrDefault().Message;
            }

            _message = _unitOfWorkAsync.Repository<ApplicationMessage>().Query().Select()
                .Where(t => t.Name == "Web_Metas");

            if (_message.Count() > 0)
            {
                ViewBag.Web_Metas = _message.FirstOrDefault().Message;
            }
            _message = _unitOfWorkAsync.Repository<ApplicationMessage>().Query().Select()
                .Where(t => t.Name == "Web_About");

            if (_message.Count() > 0)
            {
                ViewBag.Web_About = _message.FirstOrDefault().Message;
            }
            LoadSettings();
        }

        public void LoadSettings()
        {
            ViewBag.SiteName = _unitOfWorkAsync.Repository<ApplicationSetting>().Query().Select()
               .FirstOrDefault().Name;
        }
    }
}