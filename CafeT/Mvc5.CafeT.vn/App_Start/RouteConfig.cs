using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc5.CafeT.vn
{

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Home

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion Home
            
            #region Account

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "quen-mat-khau",
                defaults: new { controller = "Account", action = "ForgotPassword", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "ForgotPasswordConfirmation",
                url: "xac-nhan-lay-lai-mat-khau",
                defaults: new { controller = "Account", action = "ForgotPasswordConfirmation", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "reset-mat-khau",
                defaults: new { controller = "Account", action = "ResetPassword", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Manage",
                url: "quan-ly-tai-khoan",
                defaults: new { controller = "Manage", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion Account

            #region Articles

            routes.MapRoute(
                name: "Articles-Index",
                url: "bai-viet",
                defaults: new { controller = "Articles", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Index",
                url: "Bai-tap",
                defaults: new { controller = "Questions", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Articles-Create",
                url: "dang-bai-viet",
                defaults: new { controller = "Articles", action = "Create", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Articles-Details",
                url: "bai-viet/{id}/{seoName}",
                defaults: new { controller = "Articles", action = "Details", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "Articles-Edit",
                url: "chinh-sua-bai-viet/{id}",
                defaults: new { controller = "Articles", action = "Edit", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion Articles

            #region ApplicationUsers

            routes.MapRoute(
                name: "ApplicationUsers-Details",
                url: "thong-tin-thanh-vien/{id}",
                defaults: new { controller = "ApplicationUsers", action = "Details", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion ApplicationUsers

            #region FileModels

            routes.MapRoute(
                name: "FileModels-Index",
                url: "tai-lieu",
                defaults: new { controller = "FileModels", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "FileModels-Create",
                url: "dang-tai-lieu",
                defaults: new { controller = "FileModels", action = "Create", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "FileModels-Details",
                url: "chi-tiet-tai-lieu/{id}",
                defaults: new { controller = "FileModels", action = "Details", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion FileModels

            #region JobModels

            routes.MapRoute(
                name: "JobModels-Create",
                url: "dang-tin-tuyen-dung",
                defaults: new { controller = "JobModels", action = "Create", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "JobModels-Index",
                url: "tuyen-dung",
                defaults: new { controller = "JobModels", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "JobModels-Details",
                url: "tuyen-dung/{id}",
                defaults: new { controller = "JobModels", action = "Details", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            routes.MapRoute(
                name: "JobModels-Edit",
                url: "cap-nhat-tin-tuyen-dung/{id}",
                defaults: new { controller = "JobModels", action = "Edit", id = UrlParameter.Optional },
                namespaces: new string[] { "Mvc5.CafeT.vn.Controllers" }
            );

            #endregion JobModels

            routes.MapRoute(
                "ProductDetails", "Products/{id}/{seoName}",
                new { controller = "Products", action = "Details", seoName = "" },
                new { id = @"^\d+$" }
            );

            routes.MapRoute(
                "ProjectDetails", "Projects/{id}/{seoName}",
                new { controller = "Projects", action = "Details", seoName = "" },
                new { id = @"^\d+$" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
