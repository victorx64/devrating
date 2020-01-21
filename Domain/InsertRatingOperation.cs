namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, Envelope deletions, Id rating, Id work, Id author);
    }
}