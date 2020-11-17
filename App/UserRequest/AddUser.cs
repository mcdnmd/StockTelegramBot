using System;
using System.Collections.Generic;
using App.BotTask;
using Domain;
using Infrastructure;

namespace App.UserRequest
{
    public class AddUser : IUserRequest
    {
        private readonly string userId;
        private readonly Dictionary<string, string> parameters;
        private readonly IDataBase<IDataBaseElement> dataBase;

        public AddUser(string userId, Dictionary<string, string> parameters, IDataBase<IDataBaseElement> dataBase)
        {
            this.userId = userId;
            this.parameters = parameters;
            this.dataBase = dataBase;
        }
        
        public void Task()
        {
            var user = new User {Id = userId, PublicParserToken = parameters["api_token"]};
            dataBase.Add(user);
        }
    }
}