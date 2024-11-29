
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Transport_Management_System_App.util
{
    public static class DBConnection
    {
       
        public static string GetConnectionString()
        {
            return "Server=DESKTOP-6CA5ALC; Database=Transport Management System; Trusted_Connection=True";
        }
        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            return conn;
        }








    }
}
