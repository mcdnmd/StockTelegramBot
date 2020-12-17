using System;
using App.Logger;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class ChatStatusManager
    {
        private ILogger logger;

        public ChatStatusManager(ILogger logger)
        {
            this.logger = logger;
        }
        
        public BotReply ChangeCurrentChatStatus(IDataBase database, IUser user, ChatStatus chatStatus)
        {
            var userRecord = database.FindUser(user.Id).Result;
            logger.MakeLog(userRecord.ChatStatus.ToString());
            if (ReferenceEquals(userRecord, null) || userRecord.ChatStatus != ChatStatus.None)
                return new BotReply(user, BotReplyType.ImpossibleAction, null);
            userRecord.ChatStatus = chatStatus;
            database.UpdateUser(userRecord);
            return new BotReply(user, GetBotReplyType(chatStatus), null);
        }

        private BotReplyType GetBotReplyType(ChatStatus chatStatus)
        {
            return chatStatus switch
            {
                ChatStatus.EnterSymbolToAddNewSubscription => BotReplyType.RequestForEnterSymbol,
                ChatStatus.EnterSymbolToRemoveSubscription => BotReplyType.RequestForEnterSymbol,
                _ => throw new NotImplementedException()
            };
        }
    }
}