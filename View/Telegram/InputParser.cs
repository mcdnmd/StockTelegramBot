using System;
using System.Collections.Generic;
using App;

namespace View.Telegram
{
    public class InputParser : IInputParser
    {
        public Tuple<UserRequestType, Dictionary<string, List<string>>> ParseUserMessage(string message)
        {
            UserRequestType userRequestType;
            Dictionary<string, List<string>> parameters = default;
            switch (message)
            {
                case "/start":
                    userRequestType = UserRequestType.Start;
                    break;
                case "/help":
                    userRequestType = UserRequestType.Help;
                    break;
                case "/signin":
                    userRequestType = UserRequestType.Register;
                    break;
                case "/addsymbol":
                    userRequestType = UserRequestType.SubscribeForSymbol;
                    break;
                case "/removesymbol":
                    userRequestType = UserRequestType.UnSubscribeForSymbol;
                    break;
                case "/getprices":
                    userRequestType = UserRequestType.GetAllSymbolPrices;
                    break;
                default:
                    if (message.StartsWith('/'))
                    {
                        userRequestType = UserRequestType.UnknownCommand;
                        parameters = new Dictionary<string, List<string>>
                        {
                            ["data"] = new List<string> {message}
                        };
                    }
                    else
                    {
                        userRequestType = UserRequestType.InputRawData;
                        parameters = ParseInputData(message);
                    }
                    break;
            }

            return Tuple.Create(userRequestType, parameters);
        }

        private Dictionary<string, List<string>> ParseInputData(string messageText)
        {
            var result = new Dictionary<string, List<string>>
            {
                ["data"] = new List<string>()
            };
            var words = messageText.Split();
            foreach (var word in words)
                result["data"].Add(word);
            return result;
        }
    }
}