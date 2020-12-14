using App;
using Infrastructure;
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

        [TestCase("/register", ChatStatus.ChoseParser)]
        [TestCase("/addsymbol", ChatStatus.ChoseParser)]
        public void SimpeTest(string message, ChatStatus expected)
        {
            var databse = new MockDataBase();
            var botLogic = new BotLogic(databse);
            
            var userRequest = telegramHandler.ParseUserMessageText(telegramUser, message);
            botLogic.Execute(userRequest);
            
            Assert.AreEqual(expected, databse.Users[telegramUser.Id].ChatStatus);
        }

        private void Nothing(BotReply obj)
        {
            return;
        }
    }
}