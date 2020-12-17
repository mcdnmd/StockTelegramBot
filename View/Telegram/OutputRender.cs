using System;
using System.Collections.Generic;
using System.Text;
using App;

namespace View.Telegram
{
    public class OutputRender : IOutputRender
    {
        public string RenderReply(BotReplyType botReplyType)
        {
            var text = "SUPERMAN!";
            switch (botReplyType)
            {
                case BotReplyType.Start:
                    text = "Hi, I`m a stock parser!";
                    break;
                case BotReplyType.RequestForChoseParser:
                    text = "Enter parser";
                    break;
                case BotReplyType.RequestForEnterParserPublicToken:
                    text = "Enter your public token";
                    break;
                case BotReplyType.RequestForEnterSymbol:
                    text = "Enter symbol";
                    break;
                case BotReplyType.SingleSymbolInfo:
                    break;
                case BotReplyType.MultipleSymbolInfo:
                    break;
                case BotReplyType.UnknownParser:
                    text = "You enter unknown parser";
                    break;
                case BotReplyType.SuccessfullyRemoveSymbol:
                    text = "You successfully remove symbol";
                    break;
                case BotReplyType.SuccessfullyAddSymbol:
                    text = "You successfully add symbol";
                    break;
                case BotReplyType.SuccessfullyEnterToken:
                    text = "You successfully enter token";
                    break;
                case BotReplyType.UnknownCommand:
                    break;
                case BotReplyType.ImpossibleAction:
                    text = "Impossible Action";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return text;
        }

        public string CreateSymbolsInfo(Dictionary<string, string> botReplySymbolParameter)
        {
            var result = new StringBuilder();
            foreach (var (key, value) in botReplySymbolParameter)
            {
                if (ReferenceEquals(value, null) || value == "0")
                {
                    result.Append($"❌ {key}: not found\n");
                    continue;   
                }
                result.Append($"✔️ {key}: {value} $\n");
            }
            return result.ToString();
        }
    }
}