namespace App
{
    public class UserRequest
    {
        public long UserID;
        public UserRequestType RequestType;

        public UserRequest(long userId, UserRequestType userRequestType)
        {
            UserID = userId;
            RequestType = userRequestType;
        }
    }
}