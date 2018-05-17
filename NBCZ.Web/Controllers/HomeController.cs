using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBCZ.BLL;

namespace NBCZ.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Pub_RoleFunctionBLL user = new Pub_RoleFunctionBLL();
            return View();
        }

    }
}