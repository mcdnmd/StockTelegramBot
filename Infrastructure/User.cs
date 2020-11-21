using System.Collections.Generic;

namespace Domain
{
    public class User : IDataBaseElement
    {
        public string Id;
        public ChatStatus ChatStatus;
        public List<string> Subscriptons;
        public ParserNames ParserName;
        public string ParserToken;
    }

    public enum ChatStatus
    {
    }

    public enum ParserNames
    {
    }
}