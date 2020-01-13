namespace DevRating.Domain
{
    public interface Ratings
    {
        InsertRatingOperation InsertOperation();

        GetRatingOperation GetOperation();

        bool ContainsRatingOf(Entity author);
        bool Contains(object id);
    }
}