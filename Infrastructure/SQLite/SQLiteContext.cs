using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SQLiteContext
    {
        public async Task<List<DbDataRecord>> SendSQL(string sql)
        {
            Console.WriteLine(GetDBName("users"));
            var connection = new SqliteConnection($"Data Source={GetDBName("users")}");
            connection.Open();
            var command = new SqliteCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            var result = reader.Cast<DbDataRecord>().ToList();
            connection.Close();
            if (result.Count > 0)
                return result;
            connection.Close();
            return null;
        }
        
        

        private string GetDBName(string fileName, string extension = "db")
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var catalogs = path.Split('\\');
            var dbName = new StringBuilder();
            foreach (var catalog in catalogs)
            {
                dbName.Append($"/{catalog}");
                if (catalog == "Infrastructure")
                {
                    //dbName.Append("/SQLite");
                    break;
                }    
            }
            dbName = dbName.Remove(0, 1);
            return dbName.Append($"/{fileName}.{extension}").ToString();
        }
    }
}