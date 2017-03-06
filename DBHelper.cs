using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Resources;
using System.Reflection;

namespace MCHDAL
{
    public class DBHelper
    {
        //此处还未加入配置文件，后加
        public SqlConnection conn = new SqlConnection(Resources.SQLConnectionStr);     
    }
}
