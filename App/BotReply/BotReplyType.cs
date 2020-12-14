namespace App
{
    public enum BotReplyType
    {
        Start,
        ImpossibleAction,
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