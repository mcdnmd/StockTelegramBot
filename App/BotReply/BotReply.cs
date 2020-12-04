using System.Collections.Generic;

namespace App
{
    public class BotReply
    {
        public IUser User;
        public BotReplyType ReplyType;

        public BotReply(IUser user, BotReplyType botReplyType)
        {
            User = user;
            ReplyType = botReplyType;
        }
    }
}