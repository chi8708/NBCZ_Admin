using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace NBCZ.DAL
{
    public partial class Pub_UserRoleDAL
    {
        /// <summary>
        ///根据条件 获取用户角色列表 包括角色名
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<Pub_UserRoleExt> GetUserRoles(string strWhere, object pms = null) 
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT pur.*,pr.RoleName FROM Pub_UserRole AS pur
           LEFT JOIN Pub_Role AS PR
           ON pur.RoleCode=pr.RoleCode  ",string.IsNullOrWhiteSpace(strWhere)?"": (" Where "+ strWhere));
            using (IDbConnection conn = new SqlConnection(DapperHelper.ConnStr))
            {
                var data = conn.Query<Pub_UserRole, Pub_Role, Pub_UserRoleExt>(sql.ToString(),
                    (ur, r) => 
                    {
                      var model=ur.MapTo<Pub_UserRole,Pub_UserRoleExt>();
                      model.RoleName = r.RoleName;
                      return model;
                    }, param: pms, splitOn: "RoleCode,RoleCode");

                return data.ToList();
            }
        
        }
    }
}
