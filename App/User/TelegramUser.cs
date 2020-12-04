namespace App
{
    public class TelegramUser : IUser
    {
        public long Id { get; set; }

        public TelegramUser(long chatId)
        {
            Id = chatId;
        }
    }
}