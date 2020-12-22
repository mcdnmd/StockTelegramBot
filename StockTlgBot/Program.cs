using System;
using System.Collections.Generic;
using System.IO;
using App;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Telegram.Bot;
using View;
using Ninject;
using View.Telegram;

namespace StockTlgBot
{
    internal static class Program
    {
        private static ITelegramBotClient botClient;
        private static TelegramHandler telegramHandler;
        private static StandardKernel container = new StandardKernel();
        
        static void Main()
        {
            var settings = LoadSettings();
            if (settings.DataBaseType.Equals("PostgreHandler"))
                container.Bind<IDataBase>().To<PostgreHandler>().InSingletonScope();
            else 
                container.Bind<IDataBase>().To<SQLiteHandler>().InSingletonScope();
            container.Bind<IHttpClient>().To<HttpApiClient>().InSingletonScope();
            container.Bind<IInputParser>().To<InputParser>().InSingletonScope();
            container.Bind<IOutputRender>().To<OutputRender>().InSingletonScope();
            container.Bind<ILogger>().To<ConsoleLogger>().InSingletonScope();
            container.Bind<UserRegister>().To<UserRegister>().InSingletonScope();
            container.Bind<ChatStatusManager>().To<ChatStatusManager>().InSingletonScope();
            container.Bind<InputDataParser>().To<InputDataParser>().InSingletonScope();
            container.Bind<StockManager>().To<StockManager>().InSingletonScope();
            container.Bind<SchedulerManager>().To<SchedulerManager>().InSingletonScope();
            container.Bind<BotLogic>().To<BotLogic>().InSingletonScope();
            container.Bind<Scheduler>().To<Scheduler>().InSingletonScope();
            
            botClient = new TelegramBotClient(settings.TelegramBotToken);
            
            telegramHandler = new TelegramHandler(
                botClient, 
                container.Get<IInputParser>(), 
                container.Get<IOutputRender>());

            AddAllEventHandlers();

            telegramHandler.Initialize();
            
            container.Get<Scheduler>().Run(1);
            
            Console.WriteLine("Press key to shutdown bot");
            Console.ReadKey();
            telegramHandler.StopReciving();
        }

        private static Settings LoadSettings()
        {
            using var r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "settings.json"));
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<Settings>(json);
        }
        
        private static void AddAllEventHandlers()
        {
            container.Get<BotLogic>().OnReply += telegramHandler.BotOnReply;;
            telegramHandler.OnMessage += container.Get<BotLogic>().ExecuteUserRequest;
        }

        private class Settings
        {
            public string TelegramBotToken;
            public string DataBaseType;
        }
    }
}