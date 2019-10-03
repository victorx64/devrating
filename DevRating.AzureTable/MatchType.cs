namespace DevRating.AzureTable
{
    internal enum MatchType : byte
    {
        DeletedAnotherAuthorLine = 0,
        DeletedHisLine = 1,
        AddedNewLine = 2,
        Initialization = 3
    }
}