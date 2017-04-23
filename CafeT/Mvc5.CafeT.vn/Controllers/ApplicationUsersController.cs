using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ApplicationUsersController : BaseController
    {
        public ApplicationUsersController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
       
        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountOfPublished = _articleManager.GetAllPublished(applicationUser.UserName).Count();
            ViewBag.CountOfDrafted = _articleManager.GetAllDrafted(applicationUser.UserName).Count();
            ViewBag.User = applicationUser;
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                UserManager.Update(applicationUser);
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }
    }
}
