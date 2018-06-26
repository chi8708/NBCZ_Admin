using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBCZ.Web
{

    /// <summary>
    /// 权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationAttribute :ActionFilterAttribute
    {
        public AuthorizationAttribute(string[] functionCodes, string[] mainCode) 
        {
            this.FunctionCodes = functionCodes;
            this.MainCode = mainCode;
        }

        public AuthorizationAttribute(string[] functionCodes)
            : this(functionCodes, null)
        {
            this.FunctionCodes = functionCodes;
        }
        /// <summary>
        /// 待验证权限列表
        /// </summary>
        public string[] FunctionCodes { get; set; }

        /// <summary>
        /// 主要权限列表
        /// </summary>
        public string[] MainCode { get; set; }

        /// <summary>
        /// 权限验证拦截
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckAuth(filterContext);
            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckAuth(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {
             
                //获取该特性信息  
                var attr = GetAuthAttribute<AuthorizationAttribute>(filterContext);
                if (attr == null)
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
                AuthorizationService.Instance.CheckAuthorization(filterContext, attr.FunctionCodes, attr.MainCode);
            }


        }

        public T GetAuthAttribute<T>(ActionExecutingContext filterContext)
        {
            var obj= filterContext.ActionDescriptor.GetCustomAttributes(typeof(T), false);
            if (obj == null || obj.Length <= 0)
            {
                return default(T);
            }
            return (T)obj[0];
        }

    }





    public class AuthorizationService
    {

         private static object o=new object();
         private AuthorizationService() { }
         private static AuthorizationService instance;

        //单例模式 双重锁
         public static AuthorizationService Instance
        {
            get
            {
                if (instance==null)
                {
                    lock (o)
                    {
                        if (instance == null)
                        {
                            instance = new AuthorizationService();
                        }
                    }
                }

                return instance;
            }

        }

        /// <summary>
        /// 权限验证 同时设置page Item集合
        /// </summary>
        /// <param name="functionCodes">所查询权限集合</param>
        /// <param name="mainCode">page实际权限</param>
        /// <returns></returns>
        public  Dictionary<string, bool> CheckAuthorization(ActionExecutingContext filterContext, string[] functionCodes, string[] mainCode = null)
        {
            if (functionCodes==null||functionCodes.Length<=0)
            {
                return new Dictionary<string, bool>() {{"ok",true}};
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            Pub_FunctionBLL functionBLL = new Pub_FunctionBLL();
            var userCode = NBCZUser.UserCode;

            var context = filterContext.HttpContext;
            if (userCode == null)
            {
                context.Response.Write("<script>window.location.href='../Login/Index';</script>");
                context.Response.End();
                return dic;
            }

            dic = functionBLL.CheckUserAuth(userCode, functionCodes);
            if (dic != null)
            {
                if (mainCode == null || mainCode.Length <= 0)
                {
                    mainCode = new string[] { functionCodes.FirstOrDefault() };
                }

                bool auth = dic.Any(p => (mainCode.Contains(p.Key) && p.Value == true));//多个主权限一个符合即可
                //主权限没通过
                if (!auth)
                {
                    context.Response.Write("<script>window.location.href='../NoAuth.html';</script>");
                    context.Response.End();
                }
                SetAuthViewState(filterContext, dic);

            }

            return dic;

        }

        /// <summary>
        /// 设置权限viewstate
        /// </summary>
        /// <param name="dic"></param>
        public  void SetAuthViewState(ActionExecutingContext filterContext, Dictionary<string, bool> dic)
        {
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var controller= filterContext.Controller;
                    controller.ViewData["code" + key] = value;
                    //page.ViewState[] = value;
                }
            }
        }
    }


}