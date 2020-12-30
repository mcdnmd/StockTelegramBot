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
        private readonly StockManager stockManager;

        public SchedulerManager(IDataBase database, ILogger logger, StockManager stockManager)
        {
            this.database = database;
            this.logger = logger;
            this.stockManager = stockManager;
        }
        
        public List<BotReply> GetBotReply(SchedulerCommandType schedulerCommandType)
        {
            var usersRecords = database.GetAllUsers().Result;
            var result = new List<BotReply>();
            foreach (var userRecord in usersRecords)
            {
                switch (schedulerCommandType)
                {
                    case SchedulerCommandType.DailyPricesUpdate:
                        if (userRecord.UpdatePeriod != UpdatePeriod.Daily)
                            continue;
                        break;
                    case SchedulerCommandType.Every12HoursPricesUpdate:
                        if (userRecord.UpdatePeriod != UpdatePeriod.Every12Hours)
                            continue;
                        break;
                    case SchedulerCommandType.HourlyPricesUpdate:
                        if (userRecord.UpdatePeriod != UpdatePeriod.Hourly)
                            continue;
                        break;
                    case SchedulerCommandType.EveryHalfAnHourPricesUpdate:
                        if (userRecord.UpdatePeriod != UpdatePeriod.EveryHalfAnHour)
                            continue;
                        break;
                    case SchedulerCommandType.Every10MinutesPricesUpdate:
                        if (userRecord.UpdatePeriod != UpdatePeriod.Every10Minutes)
                            continue;
                        break;
                    default:
                        throw new NotImplementedException();
                }
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