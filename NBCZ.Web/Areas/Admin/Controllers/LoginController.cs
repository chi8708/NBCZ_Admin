using NBCZ.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBCZ.BLL;
using System.Web.Security;

namespace NBCZ.Web.Areas.Admin.Controllers
{
    public class LoginController : BaseAdminController
    {
        public Pub_UserBLL userBLL = new Pub_UserBLL();
        //
        // GET: /Admin/Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel user) 
        {
            string captcha = user.Captcha;
            if (!ModelState.IsValid)
            {
                return JavaScript("layer.msg('必填项未填写或数据格式不正确！');");
            }
            if (null != Session["Captcha"] && captcha.ToLower() != Session["Captcha"].ToString().ToLower())
            {
                return JavaScript(" layer.msg('验证码不正确');changeCaptcha();");
            }
            var dbUser = userBLL.GetList(string.Format(" StopFlag=0 AND UserName='{0}' AND UserPwd='{1}' ",
              user.UserCode,user.Password)).FirstOrDefault();

            if (dbUser == null)
            {
                return JavaScript("layer.msg('用户名或密码错误！');changeCaptcha();");
            }

            NBCZUser.WriteUser(user.UserCode);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.UserCode, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), true, FormsAuthentication.FormsCookiePath);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = ticket.CookiePath;
            Response.Cookies.Add(cookie);
            return JavaScript(string.Format("window.location.href='../Home/Index'"));
        }

        [HttpPost]
        public ActionResult LogOut() 
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            Session["Admin"] = null;
            FormsAuthentication.SignOut();
            return JavaScript(string.Format("window.location.href='../Login/Index'"));
        }

	}
}