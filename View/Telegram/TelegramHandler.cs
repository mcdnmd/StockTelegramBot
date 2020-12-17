using System;
using App;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace View.Telegram
{
    public class TelegramHandler : IUserClient
    {
        private ITelegramBotClient botClient;
        private IInputParser inputParser;
        private IOutputRender outputRender;

        public Action<UserRequest> OnMessage { get; set; }

        public TelegramHandler(ITelegramBotClient botClient, IInputParser parser, IOutputRender render)
        {
            this.botClient = botClient;
            inputParser = parser;
            outputRender = render;
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
            if (e.Message.Type != MessageType.Text)
                return;
            var userRequest = ParseUserMessageText(new TelegramUser(e.Message.Chat.Id), e.Message.Text);
            if (userRequest.RequestType == UserRequestType.UnknownCommand)
                HandleUnknownCommand(userRequest);
            else
                OnMessage(userRequest);
        }

        public UserRequest ParseUserMessageText(TelegramUser telegramUser, string message)
        {
            var (userRequestType, parameters) = inputParser.ParseUserMessage(message);
            return new UserRequest(telegramUser, userRequestType, parameters);
        }
        
        private void HandleUnknownCommand(UserRequest userRequest)
        {
            SendReply(
                new BotReply(userRequest.User, BotReplyType.UnknownCommand, null), 
                $"Unknown command {userRequest.Parameters["data"][0]}");
        }
        
        public void BotOnReply(BotReply botReply)
        {
            SendReply(botReply, outputRender.RenderReply(botReply.ReplyType));
        }
        
        public async void SendReply(BotReply botReply, string text)
        {
            ReplyKeyboardMarkup rkm;
            if (botReply.ReplyType == BotReplyType.RequestForChoseParser)
            {
                rkm = new ReplyKeyboardMarkup {Keyboard = new[] {new KeyboardButton[] {"IEXCloud", "Finhub"}}};
                await botClient.SendTextMessageAsync(
                    chatId: botReply.User.Id, 
                    replyMarkup: rkm,
                    text: text);
            }
            else if (!ReferenceEquals(botReply.SymbolParameters, null) && botReply.SymbolParameters.ContainsKey("text"))
            {
                text = outputRender.CreateSymbolsInfo(botReply.SymbolParameters["text"]);
                await botClient.SendTextMessageAsync(
                    chatId: botReply.User.Id,
                    text: text);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: botReply.User.Id, 
                    replyMarkup: new ReplyKeyboardRemove(),
                    text: text);
            }
        }
    }
}