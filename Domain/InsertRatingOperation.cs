namespace DevRating.Domain
{
    public interface InsertRatingOperation
    {
        Rating Insert(double value, Envelope<uint> deletions, Id previous, Id work, Id author);
    }
}