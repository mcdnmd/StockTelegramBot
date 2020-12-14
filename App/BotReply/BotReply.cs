using System.Collections.Generic;

namespace App
{
    public class BotReply
    {
        public IUser User;
        public BotReplyType ReplyType;
        public Dictionary<string, Dictionary<string, string>> SymbolParameters;

        public BotReply(IUser user, BotReplyType botReplyType,
            Dictionary<string, Dictionary<string, string>> symbolParameters)
        {
            User = user;
            ReplyType = botReplyType;
            SymbolParameters = symbolParameters;
        }
    }
}