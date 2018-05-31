using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web
{
    public static class FSMapper
    {
        public static FSRep MapTo(this FSRepResult repResult)
        {
            return new FSRep()
            {
                errorNo="0",
                results=repResult
            };

        }

        public static FSRep MapTo<T>(this PageDateRep<T> pageDateRep) where T : class,new()
        {
            FSRep rep = new FSRep();
            FSRepResultPage<T> fsPageDateRep = pageDateRep.MapToFSRepResultPage<T>();
            var fsRepResult= new FSRepResult() { 
                data=fsPageDateRep
             };
            rep = fsRepResult.MapTo();

            return rep;
        }

        public static FSRepResultPage<T> MapToFSRepResultPage<T>(this PageDateRep<T> pageDateRep) where T : class,new()
        {
            FSRepResultPage<T> rep = new FSRepResultPage<T>();
            rep.list = pageDateRep.data;
            rep.pages = pageDateRep.totalPage;
            rep.total = pageDateRep.count;
            rep.pageNum = pageDateRep.PageNum;
            rep.pageSize = pageDateRep.PageSize;

            return rep;
         }
    }
}