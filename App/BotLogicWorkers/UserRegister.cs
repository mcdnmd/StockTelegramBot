using System;
using System.Collections.Generic;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;

namespace App
{
    public class UserRegister
    {
        private ILogger logger;
        private readonly IDataBase database;

        public UserRegister(IDataBase database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }
        
        public BotReply Register(IUser user)
        {
            bool check;
            try
            {
                check = ReferenceEquals(database.FindUser(user.Id).Result, null);
            }
            catch (Exception)
            {
                return RegisterUser(user);
            }
            
            if (check)
                return RegisterUser(user);
            logger.MakeLog($"UserRegister: {user.Id} already exists");
            return new BotReply(user, BotReplyType.UserAlreadyRegister, null);
        }

        private BotReply RegisterUser(IUser user)
        {
            var userRecord = new UserRecord{
                Id = user.Id, 
                ChatStatus = ChatStatus.ChoseParser,
                Subscriptions = new List<string>(),
                ParserName =  ParserName.None,
                ParserToken = default,
                UpdatePeriod = UpdatePeriod.Daily 

            };
            database.AddNewUser(userRecord);
            logger.MakeLog($"UserRegister: register new user: {user.Id}");
            return new BotReply(user, BotReplyType.RequestForChoseParser, null);
        }
    }
}