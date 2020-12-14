using System;
using App;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using View;
using Ninject;

namespace StockTlgBot
{
    /*
     * StockTlgBot is a main class.
     * Данный класс запускает работу всего бота. Внутри класса будет инициализация баз данных, обработчика телеграма и
     * остальных компонент системы.
     */
    static class Program
    {
        private static ITelegramBotClient botClient;
        private static BotLogic botLogic;
        private static TelegramHandler telegramHandler;
        
        static void Main(string[] args)
        {
            const string botToken = "1412654956:AAFoRDvW1H_uG2VB9id2lPBrkYS1fLDhJ7E";
            var container = new StandardKernel();
            container.Bind<IDataBase>().To<SQLiteHandler>();
            
            //container.Bind<IUserClient>().To<TelegramHandler>();
            //botClient = container.Get<ITelegramBotClient>();
            
            botClient = new TelegramBotClient(botToken);
            botLogic = new BotLogic(container.Get<IDataBase>());
            telegramHandler = new TelegramHandler(botClient);
            
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