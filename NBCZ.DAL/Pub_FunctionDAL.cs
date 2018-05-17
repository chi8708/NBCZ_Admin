using NBCZ.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.DAL
{
    public partial class Pub_FunctionDAL
    {
        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool ChangeSotpStatus(string where, object pms)
        {
            string sql = "UPDATE Pub_Function SET StopFlag =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return DapperHelper.Excute(sql, pms) > 0;
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<string> GetUserfunctions(string userCode)
        {
            string usersql = @"select functioncode from dbo.Pub_UserFunction WHERE UserCode='{0}' ";
            usersql += "UNION";
            usersql += @" SELECT functioncode FROM dbo.Pub_RoleFunction
WHERE RoleCode=(SELECT RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode='{0}') ";

            return DapperHelper.Query<string>(string.Format(usersql, userCode));

        }
    }
}
