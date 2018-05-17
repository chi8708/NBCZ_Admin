using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web
{

    public class FSTree
    {
        public string id { get; set; }
        public string isParent { get; set; }

        public string pId { get; set; }

        public string name { get; set; }

        public string open { get; set; }

    }

}