using System;
using App.BotTask;
using Infrastructure;

namespace App
{
    public class BotLogic
    {
        public void ExecuteUserRequest(UserRequest request)
        {
            switch (request.RequestType)
            {
                case UserRequestType.Register:
                    //Register user in User DataBase
                    break;
                case UserRequestType.UnRegister:
                    // Delete all info about user in User DataBase
                    break;
                case UserRequestType.UpdateUserInfo:
                    // Change current user status for 'ChoseOptionForUpdateUserInfo'
                    break;
                case UserRequestType.SubscribeForSymbol:
                    // Change current user status for 'EnterNewSubscription'
                    break;
                case UserRequestType.UnSubscribeForSymbol:
                    // Change current user status for 'EnterDeleteSubscriptions'
                    break;
                case UserRequestType.UpdateUserInterfaceInfo:
                    // Send for user ALL info about his subscriptions
                    break;
                case UserRequestType.InputRawData:
                    // Check current user status and then parse input string
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }    
        }
    }
}