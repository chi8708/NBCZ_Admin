using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ
{
    /// <summary>
    /// 描述特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescAttribute : Attribute
    {
        public DescAttribute() { }

        public DescAttribute(string CNDesc):this(CNDesc,null)
        {
        }
        public DescAttribute(string CNDesc, string ENDesc)
        {
            this.CNDesc = CNDesc;
            this.ENDesc = ENDesc;
        }
        /// <summary>
        /// 英文描述
        /// </summary>
        public string ENDesc { get; set; }

        /// <summary>
        /// 中文描述
        /// </summary>
        public string CNDesc { get; set; }
    }

 
    /// <summary>
    /// 描述特性管理
    /// </summary>
    public static  class DescriptionService 
    {
        /// <summary>
        /// 根据字段值 获取描述信息
        /// </summary>
        public static string GetDescByValue<T>(this object val) 
        {

           // PropertyInfo[] peroperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = typeof(T).GetFields();
            var t = default(T);
            if (t is Enum)
            {
                 val = Enum.GetName(typeof(T), val);
            }
            if (val==null)
            {
                return string.Empty;
            }

            foreach (FieldInfo field in fields)
            {
                object[] objs = field.GetCustomAttributes(typeof(DescAttribute), true);
                if (objs.Length > 0)
                {
                   // return ((DescriptionAttribute)objs[0]).Description;

                    if (field.GetValue(t).ToString() == val.ToString())
                    {
                        return ((DescAttribute)objs[0]).CNDesc;
                    }
                }
            }

            return string.Empty;

        }

        /// <summary>
        /// 获取枚举的描述，该枚举必须添加有<see cref="DescriptionAttribute"/>属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>返回枚举的描述</returns>
        public static string ToDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return source.ToString();
        }

    }
}
