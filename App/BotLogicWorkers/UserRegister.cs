using System.Collections.Generic;
using App.Logger;
using Infrastructure;

namespace App
{
    public class UserRegister
    {
        private ILogger logger;

        public UserRegister(ILogger logger)
        {
            this.logger = logger;
        }
        
        public BotReply Register(IDataBase database, IUser user)
        {
            var userRecord = new UserRecord{
                Id = user.Id, 
                ChatStatus = ChatStatus.ChoseParser,
                Subscriptions = new List<string>(),
                ParserName =  ParserName.None,
                ParserToken = default,

            };
            if (ReferenceEquals(database.FindUser(user.Id), null))
            {
                database.AddNewUser(userRecord);
                logger.MakeLog($"Register new user: {user.Id}");
                return new BotReply(user, BotReplyType.RequestForChoseParser, null);
            }
            logger.MakeLog($"User {user.Id} already exists");
            return new BotReply(user, BotReplyType.UserAlreadyRegister, null);
        }
    }
}