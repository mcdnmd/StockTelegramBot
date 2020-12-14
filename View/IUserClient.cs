using System;
using App;
using Infrastructure;
using Telegram.Bot.Args;

namespace View
{
    public interface IUserClient
    {
        public Action<UserRequest> OnMessage { get; set; }
        public void OnMessageHandler(object sender, MessageEventArgs e);
        public void BotOnReply(BotReply botReply);
        public void Initialize();
        public void SendReply(BotReply botReply, string text);
    }
}