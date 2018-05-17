using NBCZ.DAL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL
{
   public partial  class Pub_UserRoleBLL
    {
       Pub_UserRoleDAL dal = new Pub_UserRoleDAL();
       /// <summary>
       ///根据条件 获取用户角色列表 包括角色名
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public List<Pub_UserRoleExt> GetUserRoles(string strWhere, object pms = null) 
       {
           return dal.GetUserRoles(strWhere, pms);
       }
    }
}
