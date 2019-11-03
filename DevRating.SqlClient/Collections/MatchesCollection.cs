using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface MatchesCollection
    {
        SqlMatch NewMatch(int first, int second, string commit, string repository, uint count);
    }
}