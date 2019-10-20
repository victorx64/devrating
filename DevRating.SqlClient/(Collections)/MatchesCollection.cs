namespace DevRating.SqlClient
{
    internal interface MatchesCollection
    {
        Match NewMatch(int first, int second, string commit, string repository, int count);
    }
}