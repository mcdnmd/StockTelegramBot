namespace App
{
    public interface IParserApi
    {
        public string Symbol { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }

        public ParserReply GetInfo(string symbol, string token);
    }
}