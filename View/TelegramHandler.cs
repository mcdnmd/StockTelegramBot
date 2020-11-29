using System;
using System.Collections.Generic;
using App.BotReply;
using App.BotTask;
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

        private async void BotOnMessage(object sender, MessageEventArgs e)
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
                        var parameters1 = new Dictionary<string, string> {["text"] = $"Unknown command:{e.Message.Text}"};
                        SendReply(new BotReply(e.Message.Chat.Id, BotReplyType.UnknownCommand, parameters1));
                        return;
                    }
                    else
                        userRequestType = UserRequestType.InputRawData;
                    break;
            }
            OnMessage(new UserRequest(userId, userRequestType));
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