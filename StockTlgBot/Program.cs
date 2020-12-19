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
        private static BotLogic botLogic;
        private static TelegramHandler telegramHandler;
        
        static void Main()
        {
            var settings = LoadSettings();
            
            var container = new StandardKernel();
            container.Bind<IDataBase>().To<SQLiteHandler>();
            container.Bind<IHttpClient>().To<HttpApiClient>();
            container.Bind<IInputParser>().To<InputParser>();
            container.Bind<IOutputRender>().To<OutputRender>();
            container.Bind<ILogger>().To<ConsoleLogger>();
            

            botClient = new TelegramBotClient(settings.TelegramBotToken);
            
            botLogic = new BotLogic(container.Get<IDataBase>(), container.Get<ILogger>());
            
            telegramHandler = new TelegramHandler(
                botClient, 
                container.Get<IInputParser>(), 
                container.Get<IOutputRender>());
            
            AddAllEventHandlers();

            var scheduler = new Scheduler(botLogic);

            telegramHandler.Initialize();
        }

        private static Settings LoadSettings()
        {
            using var r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "settings.json"));
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<Settings>(json);
        }
        
        private static void AddAllEventHandlers()
        {
            telegramHandler.OnMessage += botLogic.ExecuteUserRequest;
            botLogic.OnReply += telegramHandler.BotOnReply;
        }

        private class Settings
        {
            public string TelegramBotToken;
            public string PostgreSettings;
        }
    }
}