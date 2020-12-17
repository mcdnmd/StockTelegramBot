using System;
using System.Collections.Generic;
using App;

namespace View.Telegram
{
    public interface IInputParser
    {
        public Tuple<UserRequestType, Dictionary<string, List<string>>> ParseUserMessage(string message);
    }
}