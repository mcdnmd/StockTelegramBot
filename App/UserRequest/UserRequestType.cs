namespace App
{
    public enum UserRequestType
    {
        None,
        UnknownCommand,
        Start,
        Register,
        UpdateUserInfo,
        SubscribeForSymbol,
        UnSubscribeForSymbol,
        InputRawData,
        GetAllSymbolPrices
    }
}