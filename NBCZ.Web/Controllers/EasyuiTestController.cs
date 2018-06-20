using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }

        public ActionResult Demo() 
        {
           // return new EmptyResult();
           return View();
        }
	}
}