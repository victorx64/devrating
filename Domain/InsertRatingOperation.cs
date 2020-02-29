namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, Envelope counted, Envelope ignored, Id previous, Id work, Id author);
    }
}