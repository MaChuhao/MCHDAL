using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;


namespace MCHDAL
{
    public class TransToModel
    {
        DBHelper dbhelper = new DBHelper();
        /// <summary>
        /// 获取泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="sqlStr">要查询的T-SQL</param>
        /// <returns></returns>
        public List<T> GetList<T>( string sqlStr)where T:new()
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlStr, dbhelper.conn))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);
                T t= new T();
                return ConvertHelper<T>.ConvertToList(ds.Tables[0]);
            }
        }



    }
}
