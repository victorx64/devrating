namespace DevRating.SqlClient.Collections
{
    internal interface RatingsCollection
    {
        Entities.Rating NewRating(int author, double value, int match);
        Entities.Rating NewRating(int author, double value, int last, int match);
        Entities.Rating LastRatingOf(int author);
        bool HasRating(int author);
    }
}