using System;
using System.Collections.Generic;
using System.Linq;
using App.Logger;
using App.PublicParserAPI;
using Infrastructure;

namespace App
{
    public class StockManager
    {
        
        private ILogger logger;

        public StockManager(ILogger logger)
        {
            this.logger = logger;
        }
        
        public BotReply GetAllPrices(IDataBase database, UserRequest userRequest)
        {
            var userRecord = database.FindUser(userRequest.User.Id).Result;
            if (ReferenceEquals(userRecord, null) || userRecord.ChatStatus != ChatStatus.None)
                return new BotReply(userRequest.User, BotReplyType.ImpossibleAction, null);
            var symbols = userRecord.Subscriptions;
            if (ReferenceEquals(symbols, null))
                return new BotReply(userRequest.User, BotReplyType.ImpossibleAction, null);
            var parser = GetApiParser(userRecord.ParserName);
            var token = userRecord.ParserToken;
            var prices = MakeRequests(parser, symbols, token);
            var symbolParameters = new Dictionary<string, Dictionary<string, string>>();
            symbolParameters["text"] = prices;
            return new BotReply(userRequest.User, BotReplyType.MultipleSymbolInfo, symbolParameters);
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
    }
}