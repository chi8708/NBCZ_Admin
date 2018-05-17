using NBCZ.DAL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL
{
    public partial class  Pub_UserBLL
    {
        Pub_UserDAL dal = new Pub_UserDAL();
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "00000001";
            List<Pub_User> users = GetList("", " Id Desc ", 1);
            if (users.Count > 0)
            {
                var model = users.First();
                code = (Convert.ToInt32(model.UserCode.Remove(0, 1)) + 1).ToString().PadLeft(8, '0');
            }

            return code;
        }

        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool ChangeSotpStatus(string where, object pms)
        {

            return dal.ChangeSotpStatus(where, pms);
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Pub_User GetUserByUserName(string userName) 
        {
            var dbUser = GetList(string.Format(" StopFlag=0 AND UserName='{0}' ",
                  userName)).FirstOrDefault();

            return dbUser;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool EditPassWord(string userCode,string pwd)
        {
           return dal.EditPassWord(userCode,pwd);
         }

        
    }
}
