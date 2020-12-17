using System;

namespace App.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void MakeLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}