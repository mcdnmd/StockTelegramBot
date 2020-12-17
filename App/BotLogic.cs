using System;
using App.Logger;
using Infrastructure;

namespace App
{
    public class BotLogic
    {
        private IDataBase userDB;

        private UserRegister userRegister;
        private ChatStatusManager chatStatusManager;
        private InputDataParser inputParserData;
        private StockManager stockManager;

        public Action<BotReply> OnReply;

        public BotLogic(IDataBase userDb, ILogger logger)
        {
            userDB = userDb;
            userRegister = new UserRegister(logger);
            chatStatusManager = new ChatStatusManager(logger);
            inputParserData = new InputDataParser(logger);
            stockManager = new StockManager(logger);
        }
        
        public void ExecuteUserRequest(UserRequest request)
        {
            var reply = Execute(request);
            OnReply(reply);
        }

        public BotReply Execute(UserRequest request)
        {
            BotReply reply;
            switch (request.RequestType)
            {
                case UserRequestType.Start:
                    reply = new BotReply(request.User, BotReplyType.Start, null);
                    break;
                case UserRequestType.Register:
                    reply = userRegister.Register(userDB, request.User);
                    break;
                case UserRequestType.SubscribeForSymbol:
                    reply = chatStatusManager.ChangeCurrentChatStatus(userDB, request.User, ChatStatus.EnterSymbolToAddNewSubscription);
                    break;
                case UserRequestType.UnSubscribeForSymbol:
                    reply = chatStatusManager.ChangeCurrentChatStatus(userDB, request.User, ChatStatus.EnterSymbolToRemoveSubscription);
                    break;
                case UserRequestType.InputRawData:
                    reply = inputParserData.ParseData(userDB, request);
                    break;
                case UserRequestType.GetAllSymbolPrices:
                    reply = stockManager.GetAllPrices(userDB, request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return reply;
        }
    }
}