namespace DevRating.Domain
{
    public interface ContainsRatingOperation
    {
        bool ContainsRatingOf(Entity author);
        bool Contains(object id);
    }
}