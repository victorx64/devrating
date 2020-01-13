namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(Entity author,
            double value,
            Entity work);

        Rating Insert(Entity author,
            double value,
            Entity previous,
            Entity work);
    }
}