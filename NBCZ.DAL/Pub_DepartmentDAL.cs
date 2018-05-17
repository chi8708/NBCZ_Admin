using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.DAL
{
    public partial class Pub_DepartmentDAL
    {
          /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool ChangeSotpStatus(string where,object pms) 
        {
            string sql = "UPDATE Pub_Department SET StopFlag =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return DapperHelper.Excute(sql, pms) > 0;
        }
    }
}
