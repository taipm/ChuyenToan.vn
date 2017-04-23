using Mvc5.CafeT.vn.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mvc5.CafeT.vn
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            Application["TotalOnlineUsers"] = 0;
            JobScheduler.Start();
        }

        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] + 1;
            Application.UnLock();
        }

        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] - 1;
            Application.UnLock();
        }

        protected void Application_EndRequest()
        {
            var loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];

            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                if (loggedInUsers != null)
                {
                    loggedInUsers[userName] = DateTime.Now;
                    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                }
            }

            if (loggedInUsers != null)
            {
                foreach (var item in loggedInUsers.ToList())
                {
                    if (item.Value < DateTime.Now.AddMinutes(-10))
                    {
                        loggedInUsers.Remove(item.Key);
                    }
                }
                HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
            }

        }
    }
}
