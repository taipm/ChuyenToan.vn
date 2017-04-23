using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
