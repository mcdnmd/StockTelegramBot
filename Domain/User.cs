namespace Domain
{
    public class User : IDataBaseElement
    {
        public string Id;
        public string PublicParserName;
        public string PublicParserToken;
        public string[] SecuritiesSubscriptions;
    }
}