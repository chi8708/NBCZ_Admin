using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web
{
    /// <summary>
    /// 用户信息权限列表
    /// </summary>
    public class UserInfoAuth
    {
        /// <summary>
        /// 查看
        /// </summary>
        public const string LIST = "FC001001005";
        /// <summary>
        /// 添加
        /// </summary>
        public const string ADD = "FC001001001";
        /// <summary>
        /// 编辑
        /// </summary>
        public const string EDIT = "FC001001002";
        /// <summary>
        /// 删除
        /// </summary>
        public const string REMOVE = "FC001001003";
        /// <summary>
        /// 授权
        /// </summary>
        public const string AUTH = "FC001001004";
    }
}