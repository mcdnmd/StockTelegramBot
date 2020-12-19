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
        RequestForEnterParserPublicToken,
        RequestForEnterSymbol,
        EmptySymbolSubscriptions,
        SingleSymbolInfo,
        MultipleSymbolInfo,
        SuccessfullyRemoveSymbol,
        SuccessfullyAddSymbol,
        SuccessfullyEnterToken,
    }
}