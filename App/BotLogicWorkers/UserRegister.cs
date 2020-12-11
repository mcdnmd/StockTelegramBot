using System.Collections.Generic;
using Infrastructure;

namespace App
{
    public class UserRegister
    {
        public BotReply Register(IDataBase database, IUser user)
        {
            var userRecord = new UserRecord{
                Id = user.Id, 
                ChatStatus = ChatStatus.ChoseParser,
                Subscriptons = new List<string>(),
                ParserName =  ParserName.None,
                ParserToken = default,

            };
            database.AddNewUser(userRecord);
            return new BotReply(user, BotReplyType.RequestForChoseParser, null);
        }
    }
}