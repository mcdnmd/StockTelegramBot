using System;
using System.Timers;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class Scheduler
    {
        private readonly Timer timer;
        private readonly BotLogic botLogic;

        public Scheduler(BotLogic botLogic)
        {
            this.botLogic = botLogic;
            timer = new Timer {Interval = 60 * 1000};
            timer.Elapsed += OnElapsed;
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