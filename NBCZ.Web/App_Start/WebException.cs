using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NBCZ.Web
{
    public class WebExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {

            Task.Run(() => WriteLog(filterContext));
            base.OnException(filterContext);

        }

        /// <summary>
        /// 异常写入
        /// </summary>
        /// <param name="filterContext"></param>
        private void WriteLog(ExceptionContext filterContext) 
        {
            Exception ex = filterContext.Exception;
            var log = Common.LogFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            string message =string.Format("Message:{0}\r\nTrace:{1}",ex.Message, ex.StackTrace);
            log.Error(message);
        }
    }
}