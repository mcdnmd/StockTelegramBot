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
            switch (request.RequestType)
            {
                case UserRequestType.Register:
                    RegisterNewUser(request);
                    break;
                case UserRequestType.UnRegister:
                    // Delete all info about user in User DataBase
                    break;
                case UserRequestType.UpdateUserInfo:
                    // Change current user status for 'ChoseOptionForUpdateUserInfo'
                    break;
                case UserRequestType.SubscribeForSymbol:
                    AddNewSymbolSubscription(request);
                    break;
                case UserRequestType.UnSubscribeForSymbol:
                    RemoveSymbolSubscription(request);
                    break;
                case UserRequestType.UpdateUserInterfaceInfo:
                    // Send for user ALL info about his subscriptions
                    break;
                case UserRequestType.InputRawData:
                    ParseInputData(request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }    
        }

        private void ParseInputData(UserRequest request)
        {
            var user = userDB.FindUser(request.UserID).Result;
            var botReply = new BotReply(long.Parse(user.Id), BotReplyType.UnknownCommand, null) ;
            botReply.ReplyType = user.ChatStatus switch
            {
                ChatStatus.ChoseParser => BotReplyType.RequestForChoseParser,
                ChatStatus.EnterParserPublicToken => BotReplyType.RequestForEnterParserPublicToken,
                ChatStatus.EnterSymbolToAddNewSubscription => BotReplyType.RequestForEnterSymbol,
                ChatStatus.EnterSymbolToRemoveSubscription => BotReplyType.RequestForEnterSymbol,
                _ => throw new ArgumentOutOfRangeException()
            };
            OnReply(botReply);
        }

        private void RegisterNewUser(UserRequest request)
        {
            var id = request.UserID;
            var user = new UserDto
            {
                Id = id.ToString(),
                ChatStatus =  ChatStatus.ChoseParser
            };
            userDB.AddNewUser(user);
        }

        private void AddNewSymbolSubscription(UserRequest request)
        {
            var id = request.UserID;
            var user = userDB.FindUser(id).Result;
            
            /*
             * Почему FindUser возвращает Task<UserDto> ?!?!
             */
            
            user.ChatStatus = ChatStatus.EnterSymbolToAddNewSubscription;
            userDB.UpdateUser(user);
        }

        private void RemoveSymbolSubscription(UserRequest request)
        {
            var id = request.UserID;
            var user = userDB.FindUser(id).Result;
            user.ChatStatus = ChatStatus.EnterSymbolToRemoveSubscription;
            userDB.UpdateUser(user);
        }
    }
}