using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web
{
    /// <summary>
    /// 角色权限列表
    /// </summary>
    public class RoleAuth
    {
        /// <summary>
        /// 查看
        /// </summary>
        public const string LIST = "FC001002001";
        /// <summary>
        /// 添加
        /// </summary>
        public const string ADD = "FC001002002";
        /// <summary>
        /// 编辑
        /// </summary>
        public const string EDIT = "FC001002003";
        /// <summary>
        /// 删除
        /// </summary>
        public const string REMOVE = "FC001002004";
        /// <summary>
        /// 授权
        /// </summary>
        public const string AUTH = "FC001002005";
    }
}