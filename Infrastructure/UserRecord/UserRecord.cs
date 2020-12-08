using System.Collections.Generic;

namespace Infrastructure
{
    public class UserRecord
    {
        public long Id;
        public ChatStatus ChatStatus;
        public List<string> Subscriptons;
        public ParserName ParserName;
        public string ParserToken;
    }
}