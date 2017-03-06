using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace MCHDAL
{
    public class CreateSQLStr
    {
        /// <summary>
        /// 通用实体类存储新数据到数据库的方法 
        /// 调用此方法可获得SQL Insert语句
        /// </summary>
        /// <typeparam name="T">模板T</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string SaveSQLStr<T>(T model, string tablename)
        {

            //获得此模型的类型
            Type type = typeof(T);
            
            string fieldsName = "INSERT INTO " + tablename+ "(";
            string fieldsValue = "VALUES(";

            PropertyInfo[] propertys = model.GetType().GetProperties();
            //遍历该对象的所有属性
            foreach (PropertyInfo pi in propertys)
            {
                string name = pi.Name;
                object value = pi.GetValue(model, null);

                if(value != null)
                {
                    if(pi.PropertyType.Name.Equals("Int32"))
                    {
                        if(Int32.Parse(value.ToString()) != 0)
                        {
                            fieldsName = fieldsName + pi.Name + ',';

                            fieldsValue = fieldsValue + Int32.Parse(value.ToString()) + ',';
                        }
                    }
                    else if (pi.PropertyType.Name.Equals("DateTime"))
                    {
                        if (value.Equals("0001/1/1 0:00:000") || value.Equals("0001/1/1 0:00:00") || (DateTime)value == DateTime.MinValue)
                        {
                            continue;
                        }
                        else
                        {
                            fieldsName = fieldsName + pi.Name + ',';
                            fieldsValue = fieldsValue + "'" + value.ToString() + '\'' + ',';
                        }
                    }
                    else if (pi.PropertyType.Name.Equals("String")|| pi.PropertyType.Name.Equals("Nullable`1"))
                    {
                        fieldsName = fieldsName + pi.Name + ',';

                        fieldsValue = fieldsValue + "'" + value.ToString() + '\'' + ',';
                    }
                   
                   
                }
                
            }
            fieldsName = fieldsName.Substring(0, fieldsName.Length - 1) + ')' + ' ';

            //确保该语句返回值为主键ID
            fieldsValue = fieldsValue.Substring(0, fieldsValue.Length - 1) + ')' + "  SELECT @@IDENTITY";

            return fieldsName + fieldsValue;
        }


        /// <summary>
        /// 通用实体类更新数据库表数据的方法 
        /// 调用此方法可获得SQL Update语句
        /// </summary>
        /// <typeparam name="T">模板T</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string UpdateSQLStr<T>(T model, string tablename)
        {
            bool flag = false;
            //获得此模型的类型
            Type type = typeof(T);

            string fields = "Update " + tablename + " set ";
            string where = " where ";

            PropertyInfo[] propertys = model.GetType().GetProperties();
            //遍历该对象的所有属性
            foreach (PropertyInfo pi in propertys)
            {
                string name = pi.Name;
                object value = pi.GetValue(model, null);

                if (value != null)
                {
                    if (pi.PropertyType.Name.Equals("Int32"))
                    {
                        if (Int32.Parse(value.ToString()) != 0)
                        {
                            if (name.Equals("ID"))
                            {
                                where = where + pi.Name + "=" + Int32.Parse(value.ToString());
                                flag = true;
                            }
                            else
                            {
                                fields = fields + pi.Name + '=' + Int32.Parse(value.ToString()) + ',';                               
                            }
                        }
                    }
                    else if (pi.PropertyType.Name.Equals("DateTime"))
                    {
                        if (value.Equals("0001/1/1 0:00:000") || value.Equals("0001/1/1 0:00:00") || (DateTime)value == DateTime.MinValue)
                        {
                            continue;
                        }
                        else
                        {
                            fields = fields + pi.Name + '=' + "'" + value.ToString() + '\'' + ',';
                        }
                       
                    }
                    else if (pi.PropertyType.Name.Equals("String"))
                    {
                        fields = fields + pi.Name + '=' + "'" + value.ToString() + '\'' + ',';
                    }
                }

            }
            fields = fields.Substring(0, fields.Length - 1)  + ' ';

            if (flag)
            {
                return fields + where;
            }
            else
            {
                return "false";
            }
            
        }


        /// <summary>
        /// 通用实体类删除数据库表数据的方法
        /// 调用此方法可获得SQL Delete语句
        /// </summary>
        /// <typeparam name="T">模板T</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string DeleteSQLStr<T>(T model, string tablename)
        {
            bool flag = false;
            //获得此模型的类型
            Type type = typeof(T);

            string fields = "Delete from  " + tablename;
            string where = " where ";

            PropertyInfo[] propertys = model.GetType().GetProperties();
            //遍历该对象的所有属性
            foreach (PropertyInfo pi in propertys)
            {
                string name = pi.Name;
                object value = pi.GetValue(model, null);
                if (name.Equals("ID") && Int32.Parse(value.ToString()) != 0)
                {
                    where = where + pi.Name + "=" + Int32.Parse(value.ToString());
                    flag = true;
                }
            }

            if (flag)
            {
                return fields + where;
            }
            else
            {
                return "false";
            }

        }


        /// <summary>
        /// 通用实体类查询数据库表数据的方法
        /// 调用此方法可获得SQL Select语句
        /// </summary>
        /// <typeparam name="T">模板T</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string SelectSQLStr<T>(T model, string tableName)
        {
            string where = " where ";
            bool flag = false;
            PropertyInfo[] propertys = model.GetType().GetProperties();
            foreach(PropertyInfo pi in propertys)
            {
                string name = pi.Name;
                object value = pi.GetValue(model, null);

                if (value != null)
                {
                    if (pi.PropertyType.Name.Equals("Int32"))
                    {
                        if (Int32.Parse(value.ToString()) != 0)
                        {
                            where = where + pi.Name + "=" + Int32.Parse(value.ToString()) + " and ";
                            flag = true;                               
                        }
                    }else if (pi.PropertyType.Name.Equals("DateTime"))
                    {
                        //datetime转换时报错，因此以0001/1/1 0:00:000标识时间为空
                        if (value.Equals("0001/1/1 0:00:000")||value.Equals("0001/1/1 0:00:00")||(DateTime)value == DateTime.MinValue)
                        {
                            flag = true;
                        }
                        else
                        {
                            where = where + pi.Name + '=' + "'" + value.ToString() + '\'' + " and ";
                            flag = true;
                        }
                    }
                    else if (pi.PropertyType.Name.Equals("String"))
                    {
                        where = where + pi.Name + '=' + "'" + value.ToString() + '\'' +  " and ";
                        flag = true;
                    }


                }
            }

            where = where.Substring(0, where.Length - 4);

            if (flag)
            {
                return "SELECT * FROM " + tableName + where;
            }
            else
            {
                return "SELECT * FROM " + tableName;
            }
        }

    }
}
