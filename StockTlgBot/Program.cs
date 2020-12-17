﻿using System;
using App;
using App.Logger;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using View;
using Ninject;
using View.Telegram;

namespace StockTlgBot
{
    /*
     * StockTlgBot is a main class.
     * Данный класс запускает работу всего бота. Внутри класса будет инициализация баз данных, обработчика телеграма и
     * остальных компонент системы.
     */
    internal static class Program
    {
        private static ITelegramBotClient botClient;
        private static BotLogic botLogic;
        private static TelegramHandler telegramHandler;
        
        static void Main(string[] args)
        {
            const string botToken = "1355877173:AAEOcVr6dZGmjd5K7L2SrFNJXl2459nb4QE";
            var container = new StandardKernel();
            container.Bind<IDataBase>().To<PostgreHandler>();
            container.Bind<IHttpClient>().To<HttpApiClient>();
            container.Bind<IInputParser>().To<InputParser>();
            container.Bind<IOutputRender>().To<OutputRender>();
            container.Bind<ILogger>().To<ConsoleLogger>();

            botClient = new TelegramBotClient(botToken);
            
            botLogic = new BotLogic(container.Get<IDataBase>(), container.Get<ILogger>());
            
            telegramHandler = new TelegramHandler(
                botClient, 
                container.Get<IInputParser>(), 
                container.Get<IOutputRender>());
            
            AddAllEventHandlers();
            
            telegramHandler.Initialize();
        }

        private static void AddAllEventHandlers()
        {
            telegramHandler.OnMessage += botLogic.ExecuteUserRequest;
            botLogic.OnReply += telegramHandler.BotOnReply;
        }
    }
}