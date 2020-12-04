using System;
using App;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using View;

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
            botClient = new TelegramBotClient("1412654956:AAFoRDvW1H_uG2VB9id2lPBrkYS1fLDhJ7E");
            
            InitDb();
            
            botLogic = new BotLogic(new PostgreHandler());
            telegramHandler = new TelegramHandler(botClient);
            
            AddAllEventHandlers();
            
            telegramHandler.Initialize();
        }

        private static void AddAllEventHandlers()
        {
            telegramHandler.OnMessage += botLogic.ExecuteUserRequest;
            botLogic.OnReply += telegramHandler.BotOnReply;
        }
        
        private static void InitDb()
        {
            var dbFactory = new PostgresqlDbFactory();
            dbFactory.CreateDbContext(null).Database.Migrate();
        }
    }
}