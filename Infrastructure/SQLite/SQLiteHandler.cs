using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SQLiteHandler : IDataBase
    {
        private SQLiteContext _context = new SQLiteContext();
        public Task<UserRecord> FindUser(long id)
        {
            var result = _context.SendSQL($"SELECT Id, ChatStatus, Subscriptions, ParserName, " +
                                          $"ParserToken FROM Users WHERE Id={id};");
            return DataToUser(result.Result);
        }

        public Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL(string.Format("INSERT INTO Users (Id, ChatStatus, Subscriptions, ParserName, " +
                                     "ParserToken) VALUES ({0}, {1}, " +
                                     "'{2}', {3}, '{4}');", userRecord.Id,
                (int) userRecord.ChatStatus, subscriptions, (int)userRecord.ParserName, userRecord.ParserToken));
            return DataToUser(result.Result);
        }

        public Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL(string.Format("UPDATE Users SET ChatStatus = {0}, " +
                                                        "Subscriptions = '{1}', ParserName = {2}," +
                                                        "ParserToken = '{3}' WHERE Id = {4}", 
                (int)userRecord.ChatStatus,subscriptions , (int)userRecord.ParserName,
                userRecord.ParserToken, userRecord.Id));
            return DataToUser(result.Result);
        }

        public Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"DELETE FROM Users WHERE Id = {userRecord.Id}");

            return DataToUser(result.Result);
        }

        public async Task<UserRecord> DataToUser(DbDataRecord values)
        {
            if (values == null) return null;
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