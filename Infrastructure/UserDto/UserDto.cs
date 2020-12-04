using System.Collections.Generic;

namespace Infrastructure
{
    public class UserDto
    {
        public string Id;
        public ChatStatus ChatStatus;
        public List<string> Subscriptons;
        public ParserName ParserName;
        public string ParserToken;
    }
}