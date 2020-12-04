namespace App
{
    public class TelegramUser : IUser
    {
        public string Id { get; set; }

        public TelegramUser(long chatId)
        {
            Id = chatId.ToString();
        }
    }
}