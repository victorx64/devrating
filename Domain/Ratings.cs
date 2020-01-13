namespace DevRating.Domain
{
    public interface Ratings
    {
        InsertRatingOperation InsertOperation();
        GetRatingOperation GetOperation();
        ContainsRatingOperation ContainsOperation();
    }
}