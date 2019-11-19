namespace DevRating.Database
{
    public interface Ratings
    {
        DbRating Insert(DbObject author,
            double value,
            DbObject work);

        DbRating Insert(DbObject author,
            double value,
            DbObject previous,
            DbObject work);

        DbRating RatingOf(DbObject author);

        bool HasRatingOf(DbObject author);
    }
}