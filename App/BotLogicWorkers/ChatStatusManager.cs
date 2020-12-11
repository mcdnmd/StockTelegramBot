using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace App
{
    public class ChatStatusManager
    {
        public BotReply ChangeCurrentChatStatus(IDataBase database, IUser user, ChatStatus chatStatus)
        {
            var userRecord = database.FindUser(user.Id).Result;
            userRecord.ChatStatus = chatStatus;
            database.UpdateUser(userRecord);  
            return new BotReply(user, GetReplyType(chatStatus), null);
        }

        private BotReplyType GetReplyType(ChatStatus chatStatus)
        {
            return chatStatus switch
            {
                ChatStatus.EnterSymbolToAddNewSubscription => BotReplyType.RequestForEnterSymbol,
                ChatStatus.EnterSymbolToRemoveSubscription => BotReplyType.RequestForEnterSymbol,
                _ => throw new ArgumentOutOfRangeException(nameof(chatStatus), chatStatus, null)
            };
        }
    }
}