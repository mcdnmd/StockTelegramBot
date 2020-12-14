using System.Collections.Generic;
using Infrastructure;

namespace App
{
    public class SubscriptionManager
    {
        public void AddNewSubscription(IDataBase dataBase, IUser user, List<string> symbols)
        {
            var userRecord = dataBase.FindUser(user.Id).Result;
            foreach (var symbol in symbols)
                userRecord.Subscriptions.Add(symbol);
            userRecord.ChatStatus = ChatStatus.None; 
            dataBase.UpdateUser(userRecord);
        }

        public void RemoveSubscription(IDataBase dataBase, IUser user, List<string> symbols)
        {
            var userRecord = dataBase.FindUser(user.Id).Result;
            foreach (var symbol in symbols)
                userRecord.Subscriptions.Remove(symbol);
            userRecord.ChatStatus = ChatStatus.None;
        }
    }
}