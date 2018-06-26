using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace NBCZ.Web.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        /// <summary>
        /// 请求拦截
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //CheckAuth(filterContext);

            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckAuth(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod.ToUpper()=="GET")
            {
                //父方法
                //获得Controller类型  
                Type t = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType;
                //获得方法名  
                string actionname = filterContext.RouteData.Values["action"].ToString();
                //获取该特性信息  
                var attr = GetAuthAttribute<AuthorizationAttribute>(actionname, t);
                if (attr == null)
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
                AuthorizationService.Instance.CheckAuthorization(filterContext, attr.FunctionCodes, attr.MainCode);
            }
         

        }

        public AuthorizationAttribute GetAuthAttribute<T>(string actionname, Type t)
        {
            var method=t.GetMethods().FirstOrDefault(p=>p.Name==actionname);
            object[] obj = method.GetCustomAttributes(typeof(T), true);
            if (obj == null || obj.Length <= 0)
            {
                return null;
            }
            return obj[0] as AuthorizationAttribute;
        }
        /// <summary>
        /// view路径规则重写
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ViewResult View(string viewName, string masterName, object model)
        {

            try
            {

                string viewPath = string.Format("~/Areas/Admin/Views/{0}/{1}.cshtml", RouteData.Values["controller"], RouteData.Values["action"]);

                return base.View(viewPath, masterName, model);
            }
            catch (Exception)
            {

                return null;
            }
        }



        /// <summary>
        /// 日期序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return new CustomJsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding };
        }

        public new JsonResult Json(object data, JsonRequestBehavior jsonRequest = 0)
        {
            return new CustomJsonResult { Data = data, JsonRequestBehavior = jsonRequest };
        }

        /// <summary>
        /// 提供自定义日期格式 Json
        /// </summary>
        public class CustomJsonResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                HttpResponseBase response = context.HttpContext.Response;

                if (Data != null)
                {
                    var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };//这里使用自定义日期格式，默认是ISO8601格式        
                    response.Write(JsonConvert.SerializeObject(Data, Formatting.Indented, timeConverter));
                }
            }
        }
    }
}