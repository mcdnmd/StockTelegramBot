using Infrastructure;

namespace App
{
    public class UserRegister
    {
        public BotReply Register(IDataBase database, IUser user)
        {
            var userRecord = new UserRecord{Id = user.Id, ChatStatus = ChatStatus.ChoseParser};
            database.AddNewUser(userRecord);
            return new BotReply(user, BotReplyType.RequestForChoseParser, null);
        }
    }
}