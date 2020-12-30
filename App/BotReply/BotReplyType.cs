namespace App
{
    public enum BotReplyType
    {
        Help,
        ImpossibleAction,
        UserAlreadyRegister,
        UserNotRegistered,
        UnknownCommand,
        RequestForChoseParser,
        UnknownParser,
        RequestForEnterParserPublicTokenIEXCloud,
        RequestForEnterParserPublicTokenFinhub,
        RequestForEnterSymbol,
        EmptySymbolSubscriptions,
        SingleSymbolInfo,
        MultipleSymbolInfo,
        SuccessfullyRemoveSymbol,
        SuccessfullyAddSymbol,
        SuccessfullyEnterToken,
        NoSuchSymbolSubscription
    }
}