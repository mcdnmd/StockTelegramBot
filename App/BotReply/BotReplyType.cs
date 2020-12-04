namespace App
{
    public enum BotReplyType
    {
        Start,
        UnknownCommand,
        RequestForChoseParser,
        RequestForEnterParserPublicToken,
        RequestForEnterSymbol,
        SingleSymbolInfo,
        MultipleSymbolInfo,
        SuccessfullyRemoveSymbol,
        SuccessfullyAddSymbol,
        SuccessfullyEnterToken,
        UnknownParser
    }
}