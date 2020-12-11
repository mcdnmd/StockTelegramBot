using System;
using System.Collections.Generic;
using App;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace View
{
    public class TelegramHandler
    {
        private ITelegramBotClient botClient;

        public Action<UserRequest> OnMessage;

        public TelegramHandler(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public void Initialize()
        {
            botClient.OnMessage += BotOnMessage;
            botClient.StartReceiving();
            
            // Debug mode START
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            // Debug mode END
            
            botClient.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            var user = new TelegramUser(e.Message.Chat.Id);
            UserRequestType userRequestType;
            Dictionary<string, List<string>> parameters = default;
            switch (e.Message.Text)
            {
                case null:
                    return;
                case "/start":
                    userRequestType = UserRequestType.Start;
                    break;
                case "/register":
                    userRequestType = UserRequestType.Register;
                    break;
                case "/addsymbol":
                    userRequestType = UserRequestType.SubscribeForSymbol;
                    break;
                case "/removesymbol":
                    userRequestType = UserRequestType.UnSubscribeForSymbol;
                    break;
                default:
                    if (e.Message.Text.StartsWith('/'))
                    {
                        HandleUnknownCommand(user, e.Message.Text);
                        return;
                    }
                    else
                    {
                        userRequestType = UserRequestType.InputRawData;
                        parameters = ParseInputData(e.Message.Text);
                    }
                    break;
            }
            OnMessage(new UserRequest(user, userRequestType, parameters));
        }

        private void HandleUnknownCommand(TelegramUser user, string input)
        {
            var reply = new BotReply(user, BotReplyType.UnknownCommand, null);
            var text = $"Unknown command {input}";
            SendReply(reply, text);
        }

        private Dictionary<string, List<string>> ParseInputData(string messageText)
        {
            var result = new Dictionary<string, List<string>>();
            result["data"] = new List<string>();
            var words = messageText.Split();
            foreach (var word in words)
                result["data"].Add(word);
            return result;
        }

        public void BotOnReply(BotReply botReply)
        {
            string text = "Lox";
            switch (botReply.ReplyType)
            {
                case BotReplyType.Start:
                    text = "Hi, I`m a stock parser!";
                    break;
                case BotReplyType.RequestForChoseParser:
                    text = $"Enter parser";
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SendReply(botReply, text);
        }
        
        private async void SendReply(BotReply botReply, string text)
        {    
            
            if (!ReferenceEquals(botReply.Parameters, null) && botReply.Parameters.ContainsKey("text"))
            {
                await botClient.SendTextMessageAsync(
                    chatId: botReply.User.Id, 
                    text: botReply.Parameters["text"]);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: botReply.User.Id, 
                    text: text);
            }
        }
    }
}