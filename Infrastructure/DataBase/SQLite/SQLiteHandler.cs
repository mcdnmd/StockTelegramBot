﻿using System;
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
        public async Task<UserRecord> FindUser(long id)
        {
            var result = _context.SendSQL($"SELECT Id, ChatStatus, Subscriptions, ParserName, " +
                                          $"ParserToken, UpdatePeriod FROM Users WHERE Id={id};");
            var dbDataRecords = await result;
            if (ReferenceEquals(dbDataRecords, null))
                return null;
            return await DataToUser(dbDataRecords.First());
        }

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL("INSERT INTO Users (Id, ChatStatus, Subscriptions, ParserName, " +
                                          $"ParserToken, UpdatePeriod) VALUES ({userRecord.Id}, {(int) userRecord.ChatStatus}, " +
                                          $"'{subscriptions}', {(int) userRecord.ParserName}, '{userRecord.ParserToken}', '{(int) userRecord.UpdatePeriod}');");
            var dbDataRecords = await result;
            if (ReferenceEquals(dbDataRecords, null))
                return null;
            return await DataToUser(dbDataRecords.First());
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            var subscriptions = string.Join(';', userRecord.Subscriptions.ToArray());
            if (subscriptions.Length > 0 && subscriptions[0] == ';')
                subscriptions = subscriptions.Substring(1);
            var result = _context.SendSQL($"UPDATE Users SET ChatStatus = {(int) userRecord.ChatStatus}, " +
                                          $"Subscriptions = '{subscriptions}', ParserName = {(int) userRecord.ParserName}," +
                                          $"ParserToken = '{userRecord.ParserToken}', UpdatePeriod = '{(int) userRecord.UpdatePeriod}' WHERE Id = {userRecord.Id}");
            var dbDataRecords = await result;
            if (ReferenceEquals(dbDataRecords, null))
                return null;
            return await DataToUser(dbDataRecords.First());
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            var result = _context.SendSQL($"DELETE FROM Users WHERE Id = {userRecord.Id}");

            var dbDataRecords = await result;
            if (ReferenceEquals(dbDataRecords, null))
                return null;
            return await DataToUser(dbDataRecords.First());
        }

        public async Task<List<UserRecord>> GetAllUsers()
        {
            return _context.SendSQL($"SELECT * FROM Users").Result.Select(record => DataToUser(record).Result).ToList();
        }

        private static async Task<UserRecord> DataToUser(DbDataRecord values)
        {
            var result = new UserRecord()
            {
                Id = (long)values[0], ChatStatus = Enum.Parse<ChatStatus>(values[1].ToString()),
                Subscriptions = values[2].ToString().Split(';').ToList(), 
                ParserName = Enum.Parse<ParserName>(values[3].ToString()),
                ParserToken = values[4].ToString(),
                UpdatePeriod =  Enum.Parse<UpdatePeriod>(values[5].ToString())
            };
            return result;
        }
    }
}