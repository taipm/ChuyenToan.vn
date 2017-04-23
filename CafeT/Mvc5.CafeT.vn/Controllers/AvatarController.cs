using EyeOpen.Imaging;
using Mvc5.CafeT.vn.ModelViews;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class AvatarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string imageBase)
        {
            //byte[] byteArray = imageBytes;
            //var file = new FileContentResult(byteArray, "image/jpeg");

            return View();
        }
    }
}