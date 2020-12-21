using System;
using System.Collections.Generic;
using App.Logger;
using Infrastructure;
using Infrastructure.DataBase;

namespace App
{
    public class BotLogic
    {
        private readonly UserRegister userRegister;
        private readonly ChatStatusManager chatStatusManager;
        private readonly InputDataParser inputDataParser;
        private readonly StockManager stockManager;
        private readonly SchedulerManager schedulerManager;

        public Action<BotReply> OnReply;

        public BotLogic(UserRegister userRegister, ChatStatusManager chatStatusManager, InputDataParser inputDataParser, StockManager stockManager, SchedulerManager schedulerManager)
        {
            this.userRegister = userRegister;
            this.chatStatusManager = chatStatusManager;
            this.inputDataParser = inputDataParser;
            this.stockManager = stockManager;
            this.schedulerManager = schedulerManager;
        }
        
        public void ExecuteUserRequest(UserRequest request)
        {
            var reply = GetBotReplyOnUserRequest(request);
            OnReply(reply);
        }

        private BotReply GetBotReplyOnUserRequest(UserRequest request)
        {
            BotReply reply;
            switch (request.RequestType)
            {
                case UserRequestType.Start:
                    reply = new BotReply(request.User, BotReplyType.Help, null);
                    break;
                case UserRequestType.Register:
                    reply = userRegister.Register(request.User);
                    break;
                case UserRequestType.SubscribeForSymbol:
                    reply = chatStatusManager.ChangeCurrentChatStatus(request.User, ChatStatus.EnterSymbolToAddNewSubscription);
                    break;
                case UserRequestType.UnSubscribeForSymbol:
                    reply = chatStatusManager.ChangeCurrentChatStatus(request.User, ChatStatus.EnterSymbolToRemoveSubscription);
                    break;
                case UserRequestType.InputRawData:
                    reply = inputDataParser.ParseData(request);
                    break;
                case UserRequestType.GetAllSymbolPrices:
                    reply = stockManager.GetAllPrices(request);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return reply;
        }

        public void ExecuteSchedulerCommand(SchedulerCommand schedulerCommand)
        {
            var botReplies = GetBotRepliesOnSchedulerCommand(schedulerCommand);
            foreach (var reply in botReplies)
                OnReply(reply);
            
        }

        private IEnumerable<BotReply> GetBotRepliesOnSchedulerCommand(SchedulerCommand schedulerCommand)
        {
            var result = schedulerCommand.CommandType switch
            {
                SchedulerCommandType.SendActualPricesForUser => schedulerManager.GetBotReplyTypes(stockManager),
                _ => throw new NotImplementedException()
            };
            return result;
        }
    }
}