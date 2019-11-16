namespace DevRating.LibGit2SharpClient
{
    internal interface Hunk
    {
        Deletions Deletions();
        Additions Additions();
    }
}