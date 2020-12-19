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
                case BotReplyType.Help:
                    text = "You can control the bot by sending these commands:\n\n" +
                           "/signin - sign in\n" +
                           "/addsymbol - add new symbol(s) to your library\n" +
                           "/removesymbol - remove symbol(s) from the library\n" +
                           "/getprices - get your symbols` current prices";
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
                    text = "You successfully removed symbol";
                    break;
                case BotReplyType.SuccessfullyAddSymbol:
                    text = "You successfully added symbol";
                    break;
                case BotReplyType.SuccessfullyEnterToken:
                    text = "You successfully entered token";
                    break;
                case BotReplyType.UnknownCommand:
                    break;
                case BotReplyType.ImpossibleAction:
                    text = "Incorrect command";
                    break;
                case BotReplyType.UserAlreadyRegister:
                    text = "You have already signed in";
                    break;
                case BotReplyType.UserNotRegistered:
                    text = "Before using StocksWallet sign in using /signin";
                    break;
                case BotReplyType.EmptySymbolSubscriptions:
                    text = "You don`t have any symbols in the library. Use /addsymbol";
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