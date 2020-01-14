namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsRatingOperation : ContainsRatingOperation
    {
        public bool ContainsRatingOf(Entity author)
        {
            return true;
        }

        public bool Contains(object id)
        {
            return true;
        }
    }
}