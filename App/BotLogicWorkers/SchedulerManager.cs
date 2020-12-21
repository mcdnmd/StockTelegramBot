using System;
using System.Collections.Generic;
using App.Exceptions;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;

namespace App
{
    public class SchedulerManager
    {
        private ILogger logger;
        private readonly IDataBase database;

        public SchedulerManager(IDataBase database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }
        
        public List<BotReply> GetBotReplyTypes(StockManager stockManager)
        {
            var usersRecords = database.GetAllUsers().Result;
            var result = new List<BotReply>();
            foreach (var userRecord in usersRecords)
            {
                BotReply reply;
                try
                {
                    reply = stockManager.GetUserPricesForScheduler(userRecord);
                }
                catch (Exception e)
                {
                    logger.MakeLog(e.ToString());
                    continue;
                }
                result.Add(reply);
            }
            return result;
        }
    }
}