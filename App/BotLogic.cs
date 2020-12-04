using System;
using Infrastructure;

namespace App
{
    public class BotLogic
    {
        private IDataBase userDB;

        public Action<BotReply> OnReply;

        public BotLogic(IDataBase userDb)
        {
            userDB = userDb;
        }
        
        public void ExecuteUserRequest(UserRequest request)
        {
            BotReply reply;
            switch (request.RequestType)
            {
                case UserRequestType.Register:
                    reply = new UserRegister().Register(userDB, request.User);
                    break;
                case UserRequestType.SubscribeForSymbol:
                    reply = new ChatStatusManager().ChangeCurrentChatStatus(userDB, request.User, ChatStatus.EnterSymbolToAddNewSubscription);
                    break;
                case UserRequestType.UnSubscribeForSymbol:
                    reply = new ChatStatusManager().ChangeCurrentChatStatus(userDB, request.User, ChatStatus.EnterSymbolToRemoveSubscription);
                    break;
                case UserRequestType.InputRawData:
                    reply = new InputDataParser().ParseData(userDB, request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }    
            OnReply(reply);
        }
    }
}