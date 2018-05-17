using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NBCZ.Web.Models
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "用户编号")]
        //[EmailAddress]
        public string UserCode{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string Captcha { get; set; }
    }
}