using System;
using Infrastructure;

namespace App
{
    public class BotLogic
    {
        private IDataBase userDB;
        private IDataBase stockDB;

        public Action<BotReply> OnReply;

        public BotLogic(IDataBase userDb, IDataBase stockDb)
        {
            userDB = userDb;
            stockDB = stockDb;
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
            switch (user.ChatStatus)
            {
                case ChatStatus.ChoseParser:
                    break;
                case ChatStatus.EnterParserPublicToken:
                    break;
                case ChatStatus.EnterSymbolToAddNewSubscription:
                    break;
                case ChatStatus.EnterSymbolToRemoveSubscription:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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