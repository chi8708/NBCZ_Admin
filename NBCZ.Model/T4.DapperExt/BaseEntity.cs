using System;
using DapperExtensions.Mapper;
using Newtonsoft.Json;
using Dapper.Contrib.Extensions;

namespace NBCZ.Model
{
    public class BaseEntity<T>:ClassMapper<T> where T:class
    {
       
        public BaseEntity()
        {
            var type = typeof(T);
            Map(type.GetProperty("SchemaName")).Ignore();
            Map(type.GetProperty("Properties")).Ignore();
            Map(type.GetProperty("TableName")).Ignore();
            Map(type.GetProperty("EntityType")).Ignore();
        }
    }
}