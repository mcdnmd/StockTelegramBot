using System.Collections.Generic;

namespace App
{
    public class UserRequest
    {
        public IUser User;
        public UserRequestType RequestType;
        public Dictionary<string, List<string>> Parameters;

        public UserRequest(IUser user, UserRequestType userRequestType, Dictionary<string, List<string>> parameters)
        {
            User = user;
            RequestType = userRequestType;
            Parameters = parameters;
        }
    }
}