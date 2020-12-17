namespace App
{
    public enum BotReplyType
    {
        Start,
        ImpossibleAction,
        UserAlreadyRegister,
        UnknownCommand,
        RequestForChoseParser,
        UnknownParser,
        RequestForEnterParserPublicToken,
        RequestForEnterSymbol,
        SingleSymbolInfo,
        MultipleSymbolInfo,
        SuccessfullyRemoveSymbol,
        SuccessfullyAddSymbol,
        SuccessfullyEnterToken,
    }
}