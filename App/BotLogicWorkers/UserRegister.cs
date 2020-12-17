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
            database.AddNewUser(userRecord);
            logger.MakeLog($"Register new user: {user.Id}");
            return new BotReply(user, BotReplyType.RequestForChoseParser, null);
        }
    }
}