using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net;
using Mvc5.CafeT.vn.Models;

namespace Mvc5.CafeT.vn.Controllers
{
    public class ImagesController : Controller
    {
        //
        // GET: /Home/Index

        public ActionResult Index()
        {
            var images = new ImagesModel();
            //Read out files from the files directory
            var files = Directory.GetFiles(Server.MapPath("~/Content/img"));
            //Add them to the model
            foreach (var file in files)
                images.Images.Add(Path.GetFileName(file));

            return View(images);
        }

        [HttpPost]
        public ActionResult PreviewImage()
        {
            var bytes = new byte[0];
            ViewBag.Mime = "image/png";

            if (Request.Files.Count == 1)
            {
                bytes = new byte[Request.Files[0].ContentLength];
                Request.Files[0].InputStream.Read(bytes, 0, bytes.Length);
                ViewBag.Mime = Request.Files[0].ContentType;
            }

            ViewBag.Message = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            return PartialView();
        }

        //
        // GET: /Home/UploadImage

        public ActionResult UploadImage()
        {
            return View();
        }

        //
        // POST: /Home/UploadImage

        [HttpPost]
        public ActionResult UploadImage(UploadImageModel model)
        {
            //Check if all simple data annotations are valid
            if (ModelState.IsValid)
            {
                //Prepare the needed variables
                Bitmap original = null;
                var name = "newimagefile";
                var errorField = string.Empty;

                if (model.IsUrl)
                {
                    errorField = "Url";
                    name = GetUrlFileName(model.Url);
                    original = GetImageFromUrl(model.Url);
                }
                else if (model.IsFlickr)
                {
                    errorField = "Flickr";
                    name = GetUrlFileName(model.Flickr);
                    original = GetImageFromUrl(model.Flickr);
                }
                else if (model.File != null) // model.IsFile
                {
                    errorField = "File";
                    name = Path.GetFileNameWithoutExtension(model.File.FileName);
                    original = Bitmap.FromStream(model.File.InputStream) as Bitmap;
                }

                //If we had success so far
                if (original != null)
                {
                    var img = CreateImage(original, model.X, model.Y, model.Width, model.Height);

                    //Demo purposes only - save image in the file system
                    var fn = Server.MapPath("~/Content/img/" + name + ".png");
                    img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                    
                    //Redirect to index
                    return RedirectToAction("Index");
                }
                else //Otherwise we add an error and return to the (previous) view with the model data
                    ModelState.AddModelError(errorField, "Your upload did not seem valid. Please try again using only correct images!");
            }
            
            return View(model);
        }

        /// <summary>
        /// Gets an image from the specified URL.
        /// </summary>
        /// <param name="url">The URL containing an image.</param>
        /// <returns>The image as a bitmap.</returns>
        Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }

        /// <summary>
        /// Gets the filename that is placed under a certain URL.
        /// </summary>
        /// <param name="url">The URL which should be investigated for a file name.</param>
        /// <returns>The file name.</returns>
        string GetUrlFileName(string url)
        {
            var parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(last);
        }

        /// <summary>
        /// Creates a small image out of a larger image.
        /// </summary>
        /// <param name="original">The original image which should be cropped (will remain untouched).</param>
        /// <param name="x">The value where to start on the x axis.</param>
        /// <param name="y">The value where to start on the y axis.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <returns>The cropped image.</returns>
        Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return img;
        }
    }
}
