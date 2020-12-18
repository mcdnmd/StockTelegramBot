using System.Collections.Generic;

namespace App
{
    public class UserRequest
    {
        public IUser User { get; }
        public UserRequestType RequestType { get; }
        public Dictionary<string, List<string>> Parameters { get; }

        public UserRequest(IUser user, UserRequestType userRequestType, Dictionary<string, List<string>> parameters)
        {
            User = user;
            RequestType = userRequestType;
            Parameters = parameters;
        }
    }
}