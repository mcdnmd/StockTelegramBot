using System;
using System.Timers;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class Scheduler
    {
        private readonly Timer timer = new Timer {Interval = 10 * 60 * 1000};
        private readonly BotLogic botLogic;

        public Scheduler(BotLogic botLogic)
        {
            this.botLogic = botLogic;
        }

        public void Run(int updateIntervalInMinutes)
        {
            timer.Elapsed += OnElapsed;
            timer.Interval = 0.2 * 60 * 1000;
            timer.Enabled = true;
            
        }
        
        private void OnElapsed(object sender, EventArgs e)
        {
            timer.Enabled = false;
            botLogic.ExecuteSchedulerCommand(new SchedulerCommand(SchedulerCommandType.SendActualPricesForUser));
            timer.Enabled = true;
        }
    }
}