namespace App
{
    public enum UserRequestType
    {
        None,
        UnknownCommand,
        Start,
        Register,
        UnRegister,
        UpdateUserInfo,
        SubscribeForSymbol,
        UnSubscribeForSymbol,
        UpdateUserInterfaceInfo,
        InputRawData,
        GetAllSymbolPrices
    }
}