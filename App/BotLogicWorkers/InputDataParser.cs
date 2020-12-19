using System;
using System.Collections.Generic;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;

namespace App
{
    public class InputDataParser
    {
        private ILogger logger;

        public InputDataParser(ILogger logger)
        {
            this.logger = logger;
        }
        
        public BotReply ParseData(IDataBase database, UserRequest userRequest)
        {
            
            UserRecord userRecord;
            try
            {
                userRecord = database.FindUser(userRequest.User.Id).Result;
            }
            catch (Exception)
            {
                //{userRequest.User.Id} made impossible action {userRequest.Parameters["data"][0]}
                logger.MakeLog($"InputDataParse: {userRequest.User.Id} not found in DB");
                return new BotReply(userRequest.User, BotReplyType.UserNotRegistered, null);
            }
            if (ReferenceEquals(userRecord, null))
            {
                logger.MakeLog($"InputDataParse: {userRequest.User.Id} not found in DB");
                return new BotReply(userRequest.User, BotReplyType.UserNotRegistered, null);
            }
            
            BotReplyType type;
            switch (userRecord.ChatStatus)
            {
                case ChatStatus.ChoseParser:
                    type = EnterParserName(database, userRecord, userRequest.Parameters["data"]);
                    break;
                case ChatStatus.EnterParserPublicToken:
                    type = EnterPublicToken(database, userRecord, userRequest.Parameters["data"]);
                    break;
                case ChatStatus.EnterSymbolToAddNewSubscription:
                    type = EnterSymbolToAdd(database, userRecord, userRequest.Parameters["data"]);
                    break;
                case ChatStatus.EnterSymbolToRemoveSubscription:
                    type = EnterSymbolToRemove(database, userRecord, userRequest.Parameters["data"]);
                    break;
                case ChatStatus.None:
                    return new BotReply(userRequest.User, BotReplyType.ImpossibleAction, null);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new BotReply(userRequest.User, type, null);
        }

        private BotReplyType EnterSymbolToRemove(IDataBase database, UserRecord userRecord, List<string> symbols)
        {
            foreach (var symbol in symbols)
                userRecord.Subscriptions.Remove(symbol);
            userRecord.ChatStatus = ChatStatus.None;
            database.UpdateUser(userRecord);
            logger.MakeLog($"InputDataParse: {userRecord.Id} successfully remove symbol");
            return BotReplyType.SuccessfullyRemoveSymbol;
        }

        private BotReplyType EnterSymbolToAdd(IDataBase database, UserRecord userRecord, List<string> symbols)
        {
            foreach (var symbol in symbols)
                userRecord.Subscriptions.Add(symbol);
            userRecord.ChatStatus = ChatStatus.None; 
            database.UpdateUser(userRecord);
            logger.MakeLog($"InputDataParse: {userRecord.Id} successfully add symbol");
            return BotReplyType.SuccessfullyAddSymbol;
        }

        private BotReplyType EnterPublicToken(IDataBase database, UserRecord userRecord, List<string> publicToken)
        {
            userRecord.ParserToken = publicToken[0];
            userRecord.ChatStatus = ChatStatus.None;
            database.UpdateUser(userRecord);
            logger.MakeLog($"InputDataParse: {userRecord.Id} successfully enter public token");
            return BotReplyType.SuccessfullyEnterToken;
        }

        private BotReplyType EnterParserName(IDataBase database, UserRecord userRecord, List<string> parserNames)
        {
            var parserName = parserNames[0];
            userRecord.ChatStatus = ChatStatus.EnterParserPublicToken;
            BotReplyType botReplyType;
            switch (parserName)
            {
                case "IEXCloud":
                    userRecord.ParserName = ParserName.IEXCloud;
                    botReplyType = BotReplyType.RequestForEnterParserPublicTokenIEXCloud;
                    break;
                case "Finhub":
                    userRecord.ParserName = ParserName.Finnhub;
                    botReplyType = BotReplyType.RequestForEnterParserPublicTokenFinhub;
                    break;
                default:
                    logger.MakeLog($"InputDataParse: {userRecord.Id} chose unknown parser: {parserName}");
                    return BotReplyType.UnknownParser;
            }
            logger.MakeLog($"InputDataParse: {userRecord.Id} successfully chose parser: {parserName}");
            database.UpdateUser(userRecord);
            return botReplyType;
        }
    }
}