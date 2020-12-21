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
        private readonly ITelegramBotClient botClient;
        private readonly IInputParser inputParser;
        private readonly IOutputRender outputRender;

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
        }

        public void StopReciving()
        {
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
        
        public void BotOnReply(BotReply botReply)
        {
            SendReply(botReply, outputRender.RenderReply(botReply.ReplyType));
        }

        private UserRequest ParseUserMessageText(IUser user, string message)
        {
            var (userRequestType, parameters) = inputParser.ParseUserMessage(message);
            return new UserRequest(user, userRequestType, parameters);
        }
        
        private void HandleUnknownCommand(UserRequest userRequest)
        {
            SendReply(
                new BotReply(userRequest.User, BotReplyType.UnknownCommand, null), 
                $"Unknown command {userRequest.Parameters["data"][0]}");
        }

        public void SendReply(BotReply botReply, string text)
        {
            if (botReply.ReplyType == BotReplyType.RequestForChoseParser || botReply.ReplyType == BotReplyType.UnknownParser)
            {
                var rkm = new ReplyKeyboardMarkup {Keyboard = new[] {new KeyboardButton[] {"IEXCloud", "Finhub"}}};
                SendTelegramReplyWithMarkup(botReply.User.Id, rkm, text);
            }
            else if (!ReferenceEquals(botReply.SymbolParameters, null) && botReply.SymbolParameters.ContainsKey("text"))
            {
                text = outputRender.CreateSymbolsInfo(botReply.SymbolParameters["text"]);
                SendTelegramReplyWithoutMarkup(botReply.User.Id, text);
            }
            else
            {
                SendTelegramReplyWithoutMarkup(botReply.User.Id, text);
            }
        }

        private async void SendTelegramReplyWithoutMarkup(long id, string text)
        {
            await botClient.SendTextMessageAsync(
                chatId: id, 
                replyMarkup: new ReplyKeyboardRemove(),
                text: text,
                parseMode: ParseMode.Html);
        }

        private async void SendTelegramReplyWithMarkup(long id, ReplyKeyboardMarkup rkm, string text)
        {
            await botClient.SendTextMessageAsync(
                chatId: id, 
                replyMarkup: rkm,
                text: text,
                parseMode: ParseMode.Html);
        }
    }
}