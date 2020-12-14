using System;
using Infrastructure;

namespace App
{
    public class IEXCloudAPI : IParserApi
    {
        public string Symbol { get; set; }
        public string Token { get; set; }

        public string Url => $"https://cloud.iexapis.com/stable/stock/{Symbol}/quote?token={Token}";

        public ParserReply GetInfo(string symbol, string token)
        {
            Symbol = symbol;
            Token = token;
            Console.WriteLine(Url);
            var http = JsonParser.Parse(new HttpApiClient(Url).Get().Result);
            var result = new ParserReply
            {
                Symbol = symbol, 
                CurrentPrice = http["latestPrice"]
            };
            return result;
        }
    }
}