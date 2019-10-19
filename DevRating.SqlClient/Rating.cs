namespace DevRating.SqlClient
{
    internal interface Rating
    {
        int Id();
        int AuthorId();
        int LastRatingId();
        bool HasLastRating();
        int MatchId();
        double Value();
    }
}