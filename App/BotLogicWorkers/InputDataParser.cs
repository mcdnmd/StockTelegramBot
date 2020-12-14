using System;
using System.Collections.Generic;
using Infrastructure;

namespace App
{
    public class InputDataParser
    {
        public BotReply ParseData(IDataBase database, UserRequest userRequest)
        {
            Console.WriteLine(userRequest.User.Id + " " + userRequest.Parameters["data"][0]);
            var userRecord = database.FindUser(userRequest.User.Id).Result;
            Console.WriteLine(userRecord.ChatStatus);
            if (ReferenceEquals(userRecord, null))
                return new BotReply(userRequest.User, BotReplyType.ImpossibleAction, null);
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
            return BotReplyType.SuccessfullyRemoveSymbol;
        }

        private BotReplyType EnterSymbolToAdd(IDataBase database, UserRecord userRecord, List<string> symbols)
        {
            foreach (var symbol in symbols)
                userRecord.Subscriptions.Add(symbol);
            userRecord.ChatStatus = ChatStatus.None; 
            database.UpdateUser(userRecord);
            return BotReplyType.SuccessfullyAddSymbol;
        }

        private BotReplyType EnterPublicToken(IDataBase database, UserRecord userRecord, List<string> publicToken)
        {
            userRecord.ParserToken = publicToken[0];
            userRecord.ChatStatus = ChatStatus.None;
            database.UpdateUser(userRecord);
            return BotReplyType.SuccessfullyEnterToken;
        }

        private BotReplyType EnterParserName(IDataBase database, UserRecord userRecord, List<string> parserNames)
        {
            var parserName = parserNames[0];
            var botReplyType = BotReplyType.RequestForEnterParserPublicToken;
            userRecord.ChatStatus = ChatStatus.EnterParserPublicToken;
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
                    userRecord.ChatStatus = ChatStatus.ChoseParser;
                    break;
            }
            database.UpdateUser(userRecord);
            return botReplyType;
        }
    }
}