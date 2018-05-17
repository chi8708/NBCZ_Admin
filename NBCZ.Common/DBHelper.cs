using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NBCZ
{
    public static class DBHelper
    {
        public static string GetWhereStr(this HttpContextBase context)
        {
            string strWhere = " 1=1 ";

            var parms = context.Request.Form.AllKeys.Where(p => (p.Contains("SL_")
                || p.Contains("SEB_")) || p.Contains("SES_") || p.Contains("SEGT_") || p.Contains("SELT_") || p.Contains("SEI_") || p.Contains("SENE_") || p.Contains("SLL_") || p.Contains("SLR_"));
            foreach (string parm in parms)
            {
                var name = parm.Split('_');
                var keyPosition=name[0].Length+1;
                var fieldName = parm.Substring(keyPosition, parm.Length- keyPosition);

                var value = context.Request.Params[parm].Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                value = SqlFilter(value);

                switch (name[0])
                {
                    case "SL": strWhere += string.Format(" And {0} like '%{1}%' ", fieldName, value); break;
                    case "SLL": strWhere += string.Format(" And {0} like '%{1}' ", fieldName, value); break;
                    case "SLR": strWhere += string.Format(" And {0} like '{1}%' ", fieldName, value); break;
                    case "SEB": strWhere += string.Format(" And {0}={1} ", fieldName, value); break;
                    case "SEI": strWhere += string.Format(" And {0} in({1})", fieldName, value); break;
                    case "SES": strWhere += string.Format(" And {0}='{1}'",fieldName, value); break;
                    case "SEGT": strWhere += string.Format(" And {0}>='{1}' ",fieldName, value); break;
                    case "SELT": strWhere += string.Format(" And {0}<='{1}' ",fieldName, value); break;
                    case "SENE": strWhere += string.Format(" And {0}<>'{1}' ", fieldName, value); break;
                    default:
                        break;
                }
            }

            return strWhere;
        }

        /// <summary>
        /// 过滤危险的字符（SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SqlFilter(this string str)
        {
            var ext = new[] { "and", "exec", "insert", "select", "delete", "update", "chr", "mid", "master", "or", "truncate", "char", "declare", "join", "\r", "\n", "'" };

            if (str.Contains("'"))
            {
                str = str.Replace("'", "''");
            }
            else
            {
                if (!string.IsNullOrEmpty(str) && str.Length >= 3)
                {
                    foreach (var e in ext.Where(e => str.ToLower().IndexOf(e, StringComparison.Ordinal) != -1))
                    {
                        str = Regex.Replace(str, e, "", RegexOptions.IgnoreCase);
                    }
                }

                if (str.Length >= 128)
                    str = "";
            }


            return str;
        }
    }
}
