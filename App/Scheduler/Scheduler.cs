using System;
using System.Timers;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class Scheduler
    {
        private readonly Timer timer = new Timer();
        private readonly BotLogic botLogic;
        private readonly DateTime startDateTime = new DateTime(2000, 12, 30, 10, 0, 0);
        private readonly DateTime stopDateTime = new DateTime(2000, 12, 30, 20, 0, 0);
        private int periodCounter = 0;

        public Scheduler(BotLogic botLogic)
        {
            this.botLogic = botLogic;
        }

        public void Run(int updateIntervalInMinutes)
        {
            timer.Elapsed += OnElapsed;
            timer.Interval = 0.5 * 60 * 1000;
            timer.Enabled = true;
            
        }
        
        private void OnElapsed(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour < startDateTime.Hour ||
                DateTime.Now.Hour > stopDateTime.Hour) 
                return;
            
            timer.Enabled = false;
            botLogic.ExecuteSchedulerCommand(new SchedulerCommand(SchedulerCommandType.Every10MinutesPricesUpdate));
            timer.Enabled = true;

            if (periodCounter % 3 == 0)
            {
                timer.Enabled = false;
                botLogic.ExecuteSchedulerCommand(
                    new SchedulerCommand(SchedulerCommandType.EveryHalfAnHourPricesUpdate));
                timer.Enabled = true;
            }
            
            if (periodCounter % 6 == 0)
            {
                timer.Enabled = false;
                botLogic.ExecuteSchedulerCommand(
                    new SchedulerCommand(SchedulerCommandType.HourlyPricesUpdate));
                timer.Enabled = true;
            }
            
            if (periodCounter % (12 * 6) == 0)
            {
                timer.Enabled = false;
                botLogic.ExecuteSchedulerCommand(
                    new SchedulerCommand(SchedulerCommandType.Every12HoursPricesUpdate));
                timer.Enabled = true;
            }

            if (periodCounter % (24 * 6) == 0)
            {
                timer.Enabled = false;
                botLogic.ExecuteSchedulerCommand(
                    new SchedulerCommand(SchedulerCommandType.DailyPricesUpdate));
                timer.Enabled = true;
            }

            periodCounter += 1;
        }
    }
}