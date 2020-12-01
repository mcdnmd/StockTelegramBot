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
            var userId = e.Message.Chat.Id;
            UserRequestType userRequestType ;
            switch (e.Message.Text)
            {
                case null:
                    return;
                case "/start":
                    var parameters = new Dictionary<string, string>();
                    parameters["text"] = "Hi, bro. Let`s start make money";
                    SendReply(new BotReply(e.Message.Chat.Id, BotReplyType.UnknownCommand, parameters));
                    return;
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
                        var parameters1 = new Dictionary<string, string> {["text"] = $"Unknown command: {e.Message.Text}"};
                        BotOnReply(new BotReply(e.Message.Chat.Id, BotReplyType.UnknownCommand, parameters1));
                        return;
                    }
                    else
                        userRequestType = UserRequestType.InputRawData;
                    break;
            }
            OnMessage(new UserRequest(userId, userRequestType));
        }

        public void BotOnReply(BotReply botReply)
        {
            Dictionary<string, string> parameters;
            switch (botReply.ReplyType)
            {
                case BotReplyType.UnknownCommand:
                    parameters = null;
                    break;
                case BotReplyType.RequestForChoseParser:
                    parameters = new Dictionary<string, string> {["text"] = $"Chose parser"};
                    botReply.Parameters = parameters;
                    break;
                case BotReplyType.RequestForEnterParserPublicToken:
                    parameters = new Dictionary<string, string> {["text"] = $"Register on their website and enter your public token"};
                    botReply.Parameters = parameters;
                    break;
                case BotReplyType.RequestForEnterSymbol:
                    parameters = new Dictionary<string, string> {["text"] = $"Enter your symbol"};
                    botReply.Parameters = parameters;
                    break;
                case BotReplyType.SingleSymbolInfo:
                    break;
                case BotReplyType.MultipleSymbolInfo:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SendReply(botReply);
        }
        
        private async void SendReply(BotReply botReply)
        {
            if (botReply.Parameters.ContainsKey("text"))
            {
                await botClient.SendTextMessageAsync(
                    chatId: botReply.UserId, 
                    text: botReply.Parameters["text"]);
            }
        }
    }
}