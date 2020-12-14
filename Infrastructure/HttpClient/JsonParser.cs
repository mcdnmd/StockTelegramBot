using System.Collections.Generic;

namespace Infrastructure
{
    public static class JsonParser
    {
        public static Dictionary<string, string> Parse(string message)
        {
            var strings = message.Remove(message.Length - 1).Remove(0, 1).Replace("\"", "").Split(':', ',');
            var result = new Dictionary<string, string>();
            for (var i = 0; i < strings.Length - 1; i += 2)
                result[strings[i]] = strings[i + 1];
            return result;
        }
    }
}