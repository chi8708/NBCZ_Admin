using DapperExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBCZ.Model;

namespace NBCZ.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseAdminController
    {
        BLL.Pub_FunctionBLL _pub_functionbll = new BLL.Pub_FunctionBLL();
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult GetFunction()
        {
            List<Pub_Function> function = new List<Pub_Function>();

            //多个条件
            //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<Model.Pub_Function>(f => f.StopFlag, Operator.Eq, false));

            //一个条件
           // var predicate = Predicates.Field<Pub_Function>(f => f.StopFlag, Operator.Eq, false);

            //排序
            //IList<ISort> sort = new List<ISort>();
            //sort.Add(new Sort() { PropertyName = "StopFlag",Ascending=true });

            string functionSql = string.Format(@"select functioncode from dbo.Pub_UserFunction WHERE UserCode='{0}'
                                                UNION SELECT functioncode FROM dbo.Pub_RoleFunction
                                                WHERE RoleCode=(SELECT RoleCode FROM Pub_UserRole AS pur WHERE UserCode='{0}')",NBCZUser.UserCode);

            function = _pub_functionbll.GetList("StopFlag=0 AND MenuFlag=1 AND FunctionCode In ("+functionSql +")");
            List<Model.frMenuJson> menu = new List<Model.frMenuJson>();
            foreach (var sub in function)
            {
                Model.frMenuJson s = new Model.frMenuJson();
                s.menuId = sub.FunctionCode;
                s.menuName = sub.FunctionChina;
                s.menuIcon =string.IsNullOrWhiteSpace(sub.MenuIcon)?"fa-list":sub.MenuIcon;
                if (sub.ParentCode == "0")
                {
                    s.menuIcon = string.IsNullOrWhiteSpace(sub.MenuIcon) ? "fa-cog" : sub.MenuIcon;
                }
                else
                {
                    if (CheckSubMenuList(sub.FunctionCode, function))
                    {
                        s.menuIcon = string.IsNullOrWhiteSpace(sub.MenuIcon) ? "&#xe68e;":sub.MenuIcon;
                    }
                }
                s.menuHref = sub.URLString;
                s.parentMenuId = sub.ParentCode;
                menu.Add(s);
            }
            return Json(menu,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证是否有下级菜单
        /// </summary>
        /// <param name="FunctionCode"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckSubMenuList(string FunctionCode,List<Pub_Function> list)
        {
            foreach (var sub in list)
            {
                if (sub.ParentCode == FunctionCode && sub.MenuFlag)
                {
                    return true;
                }
            }
            return false;
        }
	}
}