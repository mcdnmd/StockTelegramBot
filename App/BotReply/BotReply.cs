using System.Collections.Generic;

namespace App
{
    public class BotReply
    {
        public long UserId;
        public BotReplyType ReplyType;
        public Dictionary<string, string> Parameters;

        public BotReply(long userId, BotReplyType botReplyType, Dictionary<string, string> parameters)
        {
            UserId = userId;
            ReplyType = botReplyType;
            Parameters = parameters;
        }
    }
}