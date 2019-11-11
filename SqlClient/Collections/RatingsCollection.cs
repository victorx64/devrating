namespace DevRating.SqlClient.Collections
{
    internal interface RatingsCollection
    {
        Entities.SqlRating NewRating(int author, double value, int match);
        Entities.SqlRating NewRating(int author, double value, int last, int match);
        Entities.SqlRating LastRatingOf(int author);
        bool HasRating(int author);
    }
}