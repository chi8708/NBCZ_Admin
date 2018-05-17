using NBCZ.DAL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL
{
    public partial class Pub_FunctionBLL
    {
        Pub_FunctionDAL dal = new Pub_FunctionDAL();
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode(string parentCode)
        {
            var code = "FC001";
            List<Pub_Function> list = GetList("ParentCode='"+parentCode+"'", " FunctionCode Desc ", 1);
            if (list.Count > 0)
            {
                var model = list.First();
                var lastNum = model.FunctionCode.Substring(model.FunctionCode.Length - 3, 3);
                code = parentCode + ((Convert.ToInt32(lastNum) + 1).ToString().PadLeft(3, '0'));
            }
            else 
            {
                code = parentCode + "001";
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
        ///  判断用户是否有某些权限
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="funcitonCodes"></param>
        /// <returns>key：code value：是否有此权限 </returns>
        public Dictionary<string, bool> CheckUserAuth(string usercode,string[] funcitonCodes)
        {
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            List<string> userFunctions = dal.GetUserfunctions(usercode);
            foreach (var code in funcitonCodes)
            {
                var isHaveCode = userFunctions.Exists(p => p == code);
                if (isHaveCode)
                {
                    dic.Add(code, true);
                }
                else
                {
                    dic.Add(code, false);
                }
            }

            return dic;
        }
    }
}
