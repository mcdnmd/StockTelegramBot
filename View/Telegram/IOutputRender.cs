using System.Collections.Generic;
using App;

namespace View.Telegram
{
    public interface IOutputRender
    {
        public string RenderReply(BotReplyType botReplyType);
        public string CreateSymbolsInfo(Dictionary<string, string> botReplySymbolParameter);
    }
}