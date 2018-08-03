using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.Common
{
    public static class TinyMapperHelper
    {
        public static TDestination MapTo<TSource,TDestination>(this TSource source) where TSource:class,new()
        {
            return TinyMapper.Map<TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            return TinyMapper.Map<TDestination>(source);
        }

        public static IList<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source) where
            TSource : class,new() 
        {
            IList<TDestination> tResult = new List<TDestination>();
            foreach (var item in source)
            {
                tResult.Add(item.MapTo<TDestination>());
            }

            return tResult;
        }
    }
}
