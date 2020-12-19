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

        public SchedulerManager(ILogger logger)
        {
            this.logger = logger;
        }
        
        public List<BotReply> GetBotReplyTypes(IDataBase dataBase, StockManager stockManager)
        {
            var usersRecords = dataBase.GetAllUsers().Result;
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