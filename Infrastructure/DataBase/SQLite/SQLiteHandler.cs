using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataBase;

namespace Infrastructure.DataBase
{
    public class SQLiteHandler : IDataBase
    {
        private SQLiteContext _context = new SQLiteContext();
        public Task<UserRecord> FindUser(long id)
        {
            var result = _context.SendSQL($"SELECT Id, ChatStatus, Subscriptions, ParserName, " +
                                          $"ParserToken FROM Users WHERE Id={id};");
            var record = result.Result;
            return (!ReferenceEquals(record, null) ? DataToUser(record[0]) : null);
        }

        public  Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL("INSERT INTO Users (Id, ChatStatus, Subscriptions, ParserName, " +
                                          $"ParserToken) VALUES ({userRecord.Id}, {(int) userRecord.ChatStatus}, " +
                                          $"'{subscriptions}', {(int) userRecord.ParserName}, '{userRecord.ParserToken}');");
            var record = result.Result;
            return (!ReferenceEquals(record, null) ? DataToUser(record[0]) : null);
        }

        public Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL($"UPDATE Users SET ChatStatus = {(int) userRecord.ChatStatus}, " +
                                          $"Subscriptions = '{subscriptions}', ParserName = {(int) userRecord.ParserName}," +
                                          $"ParserToken = '{userRecord.ParserToken}' WHERE Id = {userRecord.Id}");
            var record = result.Result;
            return (!ReferenceEquals(record, null) ? DataToUser(record[0]) : null);
        }

        public Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"DELETE FROM Users WHERE Id = {userRecord.Id}");

            var record = result.Result;
            return (!ReferenceEquals(record, null) ? DataToUser(record[0]) : null);
        }

        public async Task<List<UserRecord>> GetAllUsers()
        {
            return _context.SendSQL($"SELECT * FROM Users").Result.Select(record => DataToUser(record).Result).ToList();
        }

        private static async Task<UserRecord> DataToUser(DbDataRecord values)
        {
            if (values == null) 
                return null;
            var result = new UserRecord()
            {
                Id = (long)values[0], ChatStatus = Enum.Parse<ChatStatus>(values[1].ToString()),
                Subscriptions = values[2].ToString().Split(';').ToList(), 
                ParserName = Enum.Parse<ParserName>(values[3].ToString()),
                ParserToken = values[4].ToString()
            };
            return result;
        }
    }
}