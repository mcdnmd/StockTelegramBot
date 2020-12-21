using System;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;

using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class ChatStatusManager
    {
        private ILogger logger;
        private readonly IDataBase database;

        public ChatStatusManager(IDataBase database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }
        
        public BotReply ChangeCurrentChatStatus(IUser user, ChatStatus chatStatus)
        {
            
            UserRecord userRecord;
            try
            {
                userRecord = database.FindUser(user.Id).Result;
            }
            catch (Exception)
            {
                logger.MakeLog($"ChatStatusManager: {user.Id} not found in DB");
                return new BotReply(user, BotReplyType.UserNotRegistered, null);
            }
            if (ReferenceEquals(userRecord, null))
            {
                logger.MakeLog($"ChatStatusManager: {user.Id} not found in DB");
                return new BotReply(user, BotReplyType.UserNotRegistered, null);
            }
            if (userRecord.ChatStatus != ChatStatus.None)
            {
                logger.MakeLog($"ChatStatusManager: {user.Id} try to change {userRecord.ChatStatus} to {chatStatus}");
                return new BotReply(user, BotReplyType.ImpossibleAction, null); 
            }
            logger.MakeLog($"ChatStatusManager: {user.Id} successfully change {userRecord.ChatStatus} to {chatStatus}");
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