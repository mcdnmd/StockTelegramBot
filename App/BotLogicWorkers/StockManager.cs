using System;
using System.Collections.Generic;
using System.Linq;
using App.Exceptions;
using App.Logger;
using App.PublicParserAPI;
using Infrastructure;
using Infrastructure.DataBase;

namespace App
{
    public class StockManager
    {
        
        private ILogger logger;
        private readonly IDataBase database;
        
        public StockManager(IDataBase database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }
        
        public BotReply GetAllPrices(UserRequest userRequest)
        {
            UserRecord userRecord;
            try
            {
                userRecord = database.FindUser(userRequest.User.Id).Result;
            }
            catch (Exception)
            {
                logger.MakeLog($"StockManager: {userRequest.User.Id} not found in DB");
                return new BotReply(userRequest.User, BotReplyType.UserNotRegistered, null);
            }
            
            if (ReferenceEquals(userRecord, null))
            {
                logger.MakeLog($"StockManager: {userRequest.User.Id} not found in DB");
                return new BotReply(userRequest.User, BotReplyType.UserNotRegistered, null);
            }
            
            if (userRecord.ChatStatus != ChatStatus.None)
            {
                logger.MakeLog($"StockManager: {userRequest.User.Id} try to get prices with status {userRecord.ChatStatus}");
                return new BotReply(userRequest.User, BotReplyType.ImpossibleAction, null);
            }

            Dictionary<string, Dictionary<string, string>> symbolParameters;
            try
            {
                symbolParameters = GetSymbolParameters(userRecord);
            }
            catch (EmptySymbolSubscriptionsException e)
            {
                logger.MakeLog($"StockManager: {e}");
                return new BotReply(userRequest.User, BotReplyType.EmptySymbolSubscriptions, null);
            }
            logger.MakeLog($"StockManager: {userRequest.User.Id} successfully get prices");
            return new BotReply(userRequest.User, BotReplyType.MultipleSymbolInfo, symbolParameters);
        }

        public BotReply GetUserPricesForScheduler(UserRecord userRecord)
        {
            if (ReferenceEquals(userRecord, null) || userRecord.ChatStatus != ChatStatus.None)
                throw new UserNotExistsException();
            Dictionary<string, Dictionary<string, string>> symbolParameters;
            try
            {
                symbolParameters = GetSymbolParameters(userRecord);
            }
            catch (EmptySymbolSubscriptionsException e)
            {
                logger.MakeLog(e.ToString());
                throw;
            }
            return new BotReply(new ReplyUser{Id = userRecord.Id}, BotReplyType.MultipleSymbolInfo, symbolParameters);
        }

        private Dictionary<string, Dictionary<string, string>> GetSymbolParameters(UserRecord userRecord)
        {
            var symbols = userRecord.Subscriptions;
            if (ReferenceEquals(symbols, null) || symbols.Count == 0 || symbols[0] == "")
                throw new EmptySymbolSubscriptionsException();
            var parser = GetApiParser(userRecord.ParserName);
            var token = userRecord.ParserToken;
            var prices = MakeRequests(parser, symbols, token);
            var symbolParameters = new Dictionary<string, Dictionary<string, string>> {["text"] = prices};
            return symbolParameters;
        }
        
        private Dictionary<string, string> MakeRequests(IParserApi parser, List<string> symbols, string token)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var symbol in symbols)
            {
                var parserReply = parser.GetInfo(symbol, token);
                dictionary.Add(symbol, parserReply?.CurrentPrice);
            }

            return dictionary;
        }

        private IParserApi GetApiParser(ParserName parserApiType)
        {
            switch (parserApiType)
            {
                case ParserName.IEXCloud:
                    return new IEXCloudAPI();
                case ParserName.Finnhub:
                    return new FinhubAPI();
                default:
                    throw new NotImplementedException();
            }
        }
        
        private class ReplyUser : IUser
        {
            public long Id { get; set; }
        }
    }
}