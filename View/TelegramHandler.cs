using System;
using System.Collections.Generic;
using App;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace View
{
    public class TelegramHandler : IUserClient
    {
        private ITelegramBotClient botClient;

        public Action<UserRequest> OnMessage { get; set; }

        public TelegramHandler(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public void Initialize()
        {
            botClient.OnMessage += OnMessageHandler;
            botClient.StartReceiving();
            
            // Debug mode START
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            // Debug mode END
            
            botClient.StopReceiving();
        }

        public void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var userRequest = ParseUserMessageText(new TelegramUser(e.Message.Chat.Id), e.Message.Text);
            if (userRequest.RequestType == UserRequestType.UnknownCommand)
                HandleUnknownCommand(userRequest);
            else
                OnMessage(userRequest);
        }

        public UserRequest ParseUserMessageText(TelegramUser telegramUser, string message)
        {
            UserRequestType userRequestType;
            Dictionary<string, List<string>> parameters = default;
            switch (message)
            {
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
                    if (message.StartsWith('/'))
                    {
                        userRequestType = UserRequestType.UnknownCommand;
                        parameters = new Dictionary<string, List<string>>
                        {
                            ["data"] = new List<string> {message}
                        };
                    }
                    else
                    {
                        userRequestType = UserRequestType.InputRawData;
                        parameters = ParseInputData(message);
                    }
                    break;
            }
            return new UserRequest(telegramUser, userRequestType, parameters);
        }
        
        private void HandleUnknownCommand(UserRequest userRequest)
        {
            SendReply(
                new BotReply(userRequest.User, BotReplyType.UnknownCommand, null), 
                $"Unknown command {userRequest.Parameters["data"][0]}");
        }

        private static Dictionary<string, List<string>> ParseInputData(string messageText)
        {
            var result = new Dictionary<string, List<string>>
            {
                ["data"] = new List<string>()
            };
            var words = messageText.Split();
            foreach (var word in words)
                result["data"].Add(word);
            return result;
        }

        public void BotOnReply(BotReply botReply)
        {
            string text = "SUPERMAN!";
            switch (botReply.ReplyType)
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
            SendReply(botReply, text);
        }
        
        public async void SendReply(BotReply botReply, string text)
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