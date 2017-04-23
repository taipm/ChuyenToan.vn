using Common.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.DirectoryServices.AccountManagement;

namespace Mvc5.CafeT.vn.Helpers
{
    public static class DefaultValues
    {
        public static SelectList ItemsPerPageList
        {
            get
            {
                return (new SelectList(new List<int>() { 5, 10, 25, 50, 100 },
                selectedValue: 10));
            }
        }
}

    public static class Extensions
    {
        /// <summary>
        /// SEO Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SeoName(string name)
        {
            return Regex.Replace(name.ToLower().Replace(@"'", String.Empty), @"[^\w]+", "-");
        }
        /// <summary>
        /// Used to determine the direction of the sort identifier used when filtering lists
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="sortOrder">the current sort order being used on the page</param>
        /// <param name="field">the field that we are attaching this sort identifier to</param>
        /// <returns>MvcHtmlString used to indicate the sort order of the field</returns>
        public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string field)
        {
            if (string.IsNullOrEmpty(sortOrder) || (sortOrder.Trim() != field && sortOrder.Replace("_desc", "").Trim() != field)) return null;

            string glyph = "glyphicon glyphicon-chevron-up";
            if (sortOrder.ToLower().Contains("desc"))
            {
                glyph = "glyphicon glyphicon-chevron-down";
            }

            var span = new TagBuilder("span");
            span.Attributes["class"] = glyph;

            return MvcHtmlString.Create(span.ToString());
        }

        /// <summary>
        /// Converts a NameValueCollection into a RouteValueDictionary containing all of the elements in the collection, and optionally appends
        /// {newKey: newValue} if they are not null
        /// </summary>
        /// <param name="collection">NameValue collection to convert into a RouteValueDictionary</param>
        /// <param name="newKey">the name of a key to add to the RouteValueDictionary</param>
        /// <param name="newValue">the value associated with newKey to add to the RouteValueDictionary</param>
        /// <returns>A RouteValueDictionary containing all of the keys in collection, as well as {newKey: newValue} if they are not null</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey, string newValue)
        {
            var routeValueDictionary = new RouteValueDictionary();
            foreach (var key in collection.Keys)
            {
                if (key == null) continue;
                if (routeValueDictionary.ContainsKey(key))
                    routeValueDictionary.Remove(key);

                routeValueDictionary.Add(key, collection[key]);
            }
            if (string.IsNullOrEmpty(newValue))
            {
                routeValueDictionary.Remove(newKey);
            }
            else
            {
                if (routeValueDictionary.ContainsKey(newKey))
                    routeValueDictionary.Remove(newKey);

                routeValueDictionary.Add(newKey, newValue);
            }
            return routeValueDictionary;
        }

        public static string GetFullName(this IIdentity id)
        {
            if (id == null) return null;

            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(context, id.Name);
                return userPrincipal != null ? $"{userPrincipal.GivenName} {userPrincipal.Surname}" : null;
            }
        }
    }
}