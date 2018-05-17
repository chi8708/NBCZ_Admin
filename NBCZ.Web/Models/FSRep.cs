using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web
{
    /// <summary>
    /// fsLayui响应实体
    /// </summary>
    public class FSRep
    {
        /// <summary>
        /// "0" 成功 否则显示errorInfo
        /// </summary>
        public string errorNo { get; set; }

        public string errorInfo { get; set; }

        public FSRepResult results { get; set; }
    }

    public class FSRepResult
    {
        public object data;
    }

    public class FSRepResultPage<T> where T:class,new()
    {
      public List<T> list{get;set;}
        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public int pages { get; set; }

        public int total { get; set; }
    }
}