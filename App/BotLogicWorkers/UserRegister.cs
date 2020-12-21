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
            if (ReferenceEquals(database.FindUser(user.Id).Result, null))
            {
                var userRecord = new UserRecord{
                    Id = user.Id, 
                    ChatStatus = ChatStatus.ChoseParser,
                    Subscriptions = new List<string>(),
                    ParserName =  ParserName.None,
                    ParserToken = default,

                };
                database.AddNewUser(userRecord);
                logger.MakeLog($"UserRegister: register new user: {user.Id}");
                return new BotReply(user, BotReplyType.RequestForChoseParser, null);
            }
            logger.MakeLog($"UserRegister: {user.Id} already exists");
            return new BotReply(user, BotReplyType.UserAlreadyRegister, null);
        }
    }
}