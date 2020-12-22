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
            container.Bind<IDataBase>().To<PostgreHandler>();
            container.Bind<IHttpClient>().To<HttpApiClient>();
            container.Bind<IInputParser>().To<InputParser>();
            container.Bind<IOutputRender>().To<OutputRender>();
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<UserRegister>().To<UserRegister>();
            container.Bind<ChatStatusManager>().To<ChatStatusManager>();
            container.Bind<InputDataParser>().To<InputDataParser>();
            container.Bind<StockManager>().To<StockManager>();
            container.Bind<SchedulerManager>().To<SchedulerManager>();
            container.Bind<BotLogic>().To<BotLogic>();

            botClient = new TelegramBotClient(settings.TelegramBotToken);
            
            telegramHandler = new TelegramHandler(
                botClient, 
                container.Get<IInputParser>(), 
                container.Get<IOutputRender>());
            
            AddAllEventHandlers();
            
            container.Bind<Scheduler>().To<Scheduler>();
            
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
            public string PostgreSettings;
        }
    }
}