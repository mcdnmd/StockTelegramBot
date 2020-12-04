namespace App
{
    public enum BotReplyType
    {
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