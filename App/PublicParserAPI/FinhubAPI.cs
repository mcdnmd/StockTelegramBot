namespace App
{
    public class FinhubAPI : IParserApi
    {
        public string Symbol { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public ParserReply GetInfo(string symbol, string token)
        {
            throw new System.NotImplementedException();
        }
    }
}