using System;
using App.BotTask;

namespace App
{
    public class BotLogic
    {
        public void ExecuteTask(Action task)
        {
            task.Invoke();
        }
    }
}