namespace DevRating.Domain
{
    public interface RatingInsertOperation
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