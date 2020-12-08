using System;
using System.Collections.Generic;
using Infrastructure;

namespace App
{
    public class InputDataParser
    {
        public BotReply ParseData(IDataBase database, UserRequest userRequest)
        {
            var userRecord = database.FindUser(userRequest.User.Id).Result;
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new BotReply(userRequest.User, type, null);
        }

        private BotReplyType EnterSymbolToRemove(IDataBase database, UserRecord userRecord, List<string> symbols)
        {
            foreach (var symbol in symbols)
                userRecord.Subscriptons.Remove(symbol);
            userRecord.ChatStatus = ChatStatus.None;
            database.UpdateUser(userRecord);
            return BotReplyType.SuccessfullyRemoveSymbol;
        }

        private BotReplyType EnterSymbolToAdd(IDataBase database, UserRecord userRecord, List<string> symbols)
        {
            foreach (var symbol in symbols)
                userRecord.Subscriptons.Add(symbol);
            userRecord.ChatStatus = ChatStatus.None; 
            database.UpdateUser(userRecord);
            return BotReplyType.SuccessfullyAddSymbol;
        }

        private BotReplyType EnterPublicToken(IDataBase database, UserRecord userRecord, List<string> publicToken)
        {
            userRecord.ParserToken = publicToken[0];
            database.UpdateUser(userRecord);
            return BotReplyType.SuccessfullyEnterToken;
        }

        private BotReplyType EnterParserName(IDataBase database, UserRecord userRecord, List<string> parserNames)
        {
            var parserName = parserNames[0];
            var botReplyType = BotReplyType.RequestForEnterParserPublicToken;
            switch (parserName)
            {
                case "IEXCloud":
                    userRecord.ParserName = ParserName.IEXCloud;
                    break;
                case "Finhub":
                    userRecord.ParserName = ParserName.Finnhub;
                    break;
                default:
                    botReplyType = BotReplyType.UnknownParser;
                    break;
            }
            database.UpdateUser(userRecord);
            return botReplyType;
        }
    }
}