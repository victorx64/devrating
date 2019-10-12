namespace DevRating.AzureTable
{
    internal enum MatchType : int
    {
        DeletedAnotherAuthorLine = 0,
        DeletedHisLine = 1,
        AddedNewLine = 2,
        Initialization = 3
    }
}