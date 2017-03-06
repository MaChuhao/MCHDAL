using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;


namespace MCHDAL
{
    public class ConvertHelper<T> where T:new()
    {
        /// <summary>
        /// 将DataTable转换为泛型数据
        /// Model实体的属性名必须与数据库中的列名完全一致
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns></returns>
        public static List<T> ConvertToList(DataTable dt)
        {
            //定义集合
            List<T> ts = new List<T>();
            //获得此模型的类型
            Type type = typeof(T);
            string tempName = string.Empty;
            //遍历datatable中所有行
            foreach(DataRow dr in dt.Rows)
            {
                T t = new T();
                //获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach(PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    //检查DataTable是否包含此项
                    if(dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;//不可写，跳出
                        object value = dr[tempName];
                        //非空即赋值
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

      
    }
}
