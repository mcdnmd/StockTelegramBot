using System;
using System.Collections.Generic;
using Domain;
using App;
using App.BotTask;
using App.PublicParserAPI;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace TlgImitation
{
    class Program
    {
        private static IDataBase<IDataBaseElement> dataBase;
        private static BotLogic telegramLogic;

        public static void Main(string[] args)
        {
            InitAllSystem();
            Console.WriteLine("Start Imitation!");
            CreateNewUser();
        }

        private static void CreateNewUser()
        {
            const string userId = "test_user_t0k3n";
            var parameters = new Dictionary<string, string>
            {
                ["api_type"] = PublicParserAPIType.IEXCloud.ToString(),
                ["api_key"] = "pk_e9238088414a47c5bc539986a96feb9b"
            };

            var userRequest = new AddUser(userId, parameters, dataBase);
            
            telegramLogic.ExecuteTask(userRequest.Task);
        }

        private static void InitAllSystem()
        {
            dataBase = new PostgreHandler<IDataBaseElement>();
            telegramLogic = new BotLogic();
        }
    }
}