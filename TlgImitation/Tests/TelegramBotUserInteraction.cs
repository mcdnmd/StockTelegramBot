using App;
using NUnit.Framework;
using Telegram.Bot.Args;
using TlgImitation.DbImitation;
using View;

namespace TlgImitation.Tests
{
    [TestFixture]
    public class TelegramBotUserInteraction
    {
        private TelegramHandler telegramHandler;
        private BotLogic botLogic;
        private TelegramUser telegramUser;

        [SetUp]
        public void SetUp()
        {
            telegramUser = new TelegramUser(1337);
            
            botLogic = new BotLogic(new MockDataBase());
            telegramHandler = new TelegramHandler(null);
            
            telegramHandler.OnMessage += botLogic.ExecuteUserRequest;
            botLogic.OnReply += Nothing;
        }

        [TestCase("/start", UserRequestType.Start)]
        [TestCase("/register", UserRequestType.Register)]
        [TestCase("/addsymbol", UserRequestType.SubscribeForSymbol)]
        [TestCase("/removesymbol", UserRequestType.UnSubscribeForSymbol)]
        [TestCase("/not_implemented_command", UserRequestType.UnknownCommand)]
        [TestCase("how it works?", UserRequestType.InputRawData)]
        [TestCase("how it /works", UserRequestType.InputRawData)]
        public void ConvertRawDataIntoUserRequestType(string message, UserRequestType expected)
        {
            var userRequest = telegramHandler.ParseUserMessageText(telegramUser, message);
            Assert.AreEqual(expected, userRequest.RequestType);
        }

        private void Nothing(BotReply obj)
        {
            return;
        }
    }
}