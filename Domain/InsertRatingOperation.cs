namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, ObjectEnvelope deletions, Entity previous, Entity work, Entity author);
    }
}