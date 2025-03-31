using SqlSugar;
using System.Configuration;

namespace SquenceMange.DataBase
{
    public class DbContext
    {
        public static SqlSugarClient Instance => new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
    }
}