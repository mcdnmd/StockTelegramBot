using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SQLiteHandler : IDataBase
    {
        private SQLiteContext _context = new SQLiteContext();
        public Task<UserRecord> FindUser(long id)
        {
            var result = _context.SendSQL($"SELECT * FROM Users WHERE Id={id}");
            
            return DataToUser(result.Result);
        }

        public Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"INSERT INTO Users (Id, ChatStatus, Subscriptions, ParserName, " +
                                     $"ParserToken) VALUES ({userRecord.Id},{(int) userRecord.ChatStatus}," +
                                     $"{userRecord.Subscriptons.ToString()},{(int) userRecord.ParserName},{userRecord.ParserToken})");
            return DataToUser(result.Result);
        }

        public Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"UPDATE Users SET ChatStatus = {(int)userRecord.ChatStatus}, " +
                                          $"Subscriptions = {userRecord.Subscriptons.ToString()}, ParserName = {(int)userRecord.ParserName}," +
                                          $"ParserToken = {userRecord.ParserToken} WHERE Id = {userRecord.Id}");
            return DataToUser(result.Result);
        }

        public Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"DELETE FROM Users WHERE Id = {userRecord.Id}");

            return DataToUser(result.Result);
        }

        public async Task<UserRecord> DataToUser(SqliteDataReader reader)
        {
            var result = new UserRecord();
            return result;
        }
    }
}