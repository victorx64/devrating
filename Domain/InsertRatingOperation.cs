namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, DbParameter deletions, Entity previous, Entity work, Entity author);
    }
}