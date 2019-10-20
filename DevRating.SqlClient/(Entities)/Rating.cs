namespace DevRating.SqlClient
{
    internal interface Rating : IdentifiableObject
    {
        int AuthorId();
        int LastRatingId();
        bool HasLastRating();
        int MatchId();
        double Value();
    }
}