namespace Domain
{
    public class User : IDataBaseElement
    {
        public string Id;
        public string Stock;
        public string Token;
        public string[] Symbols; 
    }
}