using System.Collections.Generic;

namespace Infrastructure
{
    public class UserDto
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