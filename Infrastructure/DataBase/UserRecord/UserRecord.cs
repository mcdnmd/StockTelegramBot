using System.Collections.Generic;

namespace Infrastructure.DataBase
{
    public class UserRecord
    {
        public long Id { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public List<string> Subscriptions { get; set; }
        public ParserName ParserName { get; set; }
        public string ParserToken { get; set; }
    }
}