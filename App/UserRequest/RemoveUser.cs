using System;
using System.Collections.Generic;
using App.BotTask;
using Domain;
using Infrastructure;

namespace App.UserRequest
{
    public class RemoveUser : IUserRequest
    {
        public Action Task { get; }

        public RemoveUser(string userId, Dictionary<string, string> parameters, IDataBase<IDataBaseElement> dataBase)
        {
            Task = CreateTask(userId, parameters, dataBase);
        }
        
        private static Action CreateTask(string userId, Dictionary<string, string> parameters,
            IDataBase<IDataBaseElement> dataBase)
        {
            return delegate
            {
                var user = new User {Id = userId, Token = parameters["api_token"]};
                dataBase.Remove(user);
            };
        }
    }
}