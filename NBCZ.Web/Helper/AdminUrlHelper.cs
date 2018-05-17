using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// Admin区域url扩展
    /// </summary>
    public static class AdminUrlHelper
    {

        public static string ActionAdmin(this UrlHelper urlHelper, string actionName, object routeValues=null) 
        {
            if (routeValues==null)
            {
                routeValues = new { Area = "Admin" };
            }
            return urlHelper.Action(actionName, routeValues);
        }

        public static string ActionAdmin(this UrlHelper urlHelper, string actionName,string controllerName, object routeValues = null)
        {
            if (routeValues == null)
            {
                routeValues = new { Area = "Admin" };
            }
            return urlHelper.Action(actionName, controllerName, routeValues);
        }
    }
}