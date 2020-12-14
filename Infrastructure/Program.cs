using System;
using System.Collections.Generic;

namespace Infrastructure
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new SQLiteHandler();
            a.AddNewUser(new UserRecord()
            {
                Id = 3, Subscriptons = new List<string>(){"ggg"},ChatStatus = ChatStatus.ChoseParser,
                ParserName = ParserName.Finnhub, ParserToken = "lflflf"
            });
            a.FindUser(1);
        }
    }
}