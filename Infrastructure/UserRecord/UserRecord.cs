using System.Collections.Generic;

namespace Infrastructure
{
    public class UserRecord
    {
        public long Id { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public List<string> Subscriptons { get; set; }
        public ParserName ParserName { get; set; }
        public string ParserToken { get; set; }
    }
}