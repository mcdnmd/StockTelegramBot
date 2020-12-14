using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SQLiteContext
    {
        public async Task<SqliteDataReader> SendSQL(string sql)
        {
            Console.WriteLine(GetDBName("users"));
            var connection = new SqliteConnection($"Data Source={GetDBName("Users")}");
            connection.Open();
            var command = new SqliteCommand(sql, connection);
            var reader = command.ExecuteReader();
            connection.Close();
            return reader; 
        }
        
        public string GetDBName(string fileName, string extension = "db")
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var catalogs = path.Split('\\');
            var dbName = new StringBuilder();
            foreach (var catalog in catalogs)
            {
                dbName.Append($"/{catalog}");
                if (catalog == "Infrastructure")
                {
                    break;
                }    
            }
            dbName = dbName.Remove(0, 1);
            return dbName.Append($"/{fileName}.{extension}").ToString();
        }
    }
}