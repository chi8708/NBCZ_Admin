using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBCZ.Common;
using System.Drawing;

namespace NBCZ.Web.Controllers
{
    public class CaptchaController : Controller
    {
        //
        // GET: /Captcha/
        public void Index()
        {

            //#region 验证码
            //Session["Captcha"] = outstring.ToLower();
            //#endregion

            //Response.ClearContent();
            //Response.BinaryWrite(captcha);

            //cts修改只产生数字验证
            string s = ValidateCode.GetRndNum();
            Session["Captcha"] = s.ToLower();
            using (Bitmap img = ValidateCode.CreateImages(s, "ch"))
            {
                Response.ClearContent();
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
	}
}