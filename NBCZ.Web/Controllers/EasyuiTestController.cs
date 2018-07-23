using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NBCZ.Web.Controllers
{
    public class EasyuiTestController : Controller
    {
        //
        // GET: /EasyuiTest/
        public ActionResult Index()
        {
            BindLeftMenu();
            return View();
        }

        public ActionResult Demo() 
        {
           // return new EmptyResult();
           return View();
        }

        /// <summary>
        /// 绑定左边菜单
        /// </summary>
        private void BindLeftMenu()
        {
     
            var funs =  new Pub_FunctionBLL().GetList("StopFlag=0 AND MenuFlag=1 ");

            var navHtml = new StringBuilder();
            var funs1 = funs.Where(p => string.IsNullOrEmpty(p.ParentCode)||p.ParentCode=="0");
            foreach (var item in funs1)
            {
                navHtml.AppendFormat("<div title='{0}'  data-options=\"iconCls:'fa fa-navicon'\">", item.FunctionChina);
                navHtml.Append(BindLeftMenuChild(funs, item));
                navHtml.AppendFormat("</div>");
            }
            ViewBag.NavHtml = navHtml;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 绑定子菜单
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        //StringBuilder childNav = new StringBuilder();
        private StringBuilder BindLeftMenuChild(IEnumerable<Pub_Function> funs, Pub_Function now)
        {
            var childNav = new StringBuilder();
            var child = funs.Where(p => p.ParentCode == now.FunctionCode);
            childNav.AppendFormat("");
            if (child != null && child.Count() > 0)
            {
                foreach (var item in child)
                {
                    var itemChildren = funs.Where(p => p.ParentCode == item.FunctionCode);
                    if (itemChildren != null && itemChildren.Count() > 0)
                    {
                        childNav.AppendFormat("<div  class='easyui-accordion menu-sub' data-options='border:false,fit:false,selected:true' style='padding-left:10px;'><div title='{0}' data-options=\"iconCls:'fa fa-bars'\">", item.FunctionChina);
                        childNav.Append(BindLeftMenuChild(funs, item));
                        childNav.AppendFormat("</div></div>");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.URLString))
                        {
                            childNav.AppendFormat(" <a href='javascript:void(0);' src='{1}' class='fun-search cs-navi-tab'>{0}</a>", item.FunctionChina, item.URLString);
                        }

                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(now.URLString))
                {
                    childNav.AppendFormat(" <a href='javascript:void(0);' src='{1}' class='fun-search cs-navi-tab'>{0}</a>", now.FunctionChina, now.URLString);
                }
            }
            childNav.AppendFormat("");
            return childNav;
        }
	}
}