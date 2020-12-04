using System.Collections.Generic;

namespace App
{
    public class BotReply
    {
        public IUser User;
        public BotReplyType ReplyType;
        public Dictionary<string, string> Parameters;

        public BotReply(IUser user, BotReplyType botReplyType, Dictionary<string, string> parameters)
        {
            User = user;
            ReplyType = botReplyType;
            Parameters = parameters;
        }
    }
}