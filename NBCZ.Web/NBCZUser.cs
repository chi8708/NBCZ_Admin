using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ
{
    public class NBCZUser : System.Web.SessionState.IRequiresSessionState
    {
        public NBCZUser() { }

        public static string UserCode
        {
            get
            {
                var user= GetPubUser();
                return user.UserCode;               
            }
        }

        public static string UserName
        {
            get
            {
                var user = GetPubUser();
                return user.UserName;   
            }
        }

        public static string DeptCode
        {
            get
            {
                var user = GetPubUser();
                return user.DeptCode; 
            }
        }


        public static string MobilePhone
        {
            get
            {
                var user = GetPubUser();
                return user.MobilePhone; 
            }
        }


        public static void WriteUser(string userName)
        {
            LoginAdmin admin = new LoginAdmin();
            var pubUser = new Pub_UserBLL().GetUserByUserName(userName);
            var context = HttpContext.Current;
            if (pubUser!=null)
            {
                admin.UserCode = pubUser.UserCode;
                admin.UserName = pubUser.UserName;
                admin.MobilePhone = pubUser.Tel;
                admin.DeptCode = pubUser.DeptCode;
            }
            context.Session["Admin"] = admin;
        }


        private static LoginAdmin GetPubUser()
        {
            var context=HttpContext.Current;
            LoginAdmin admin = new LoginAdmin();
            if (context.Session["Admin"]==null)
            {
                if (!string.IsNullOrEmpty(context.User.Identity.Name))
                {
                     WriteUser(context.User.Identity.Name);
                }
            }
            admin= context.Session["Admin"] as LoginAdmin??new LoginAdmin();         

            return admin;
        }


        private class LoginAdmin
        {
            public string UserCode { get; set; }
            public string  UserName { get; set; }

            public string DeptCode { get; set; }

            public string MobilePhone { get; set; }
        }
    }
}
