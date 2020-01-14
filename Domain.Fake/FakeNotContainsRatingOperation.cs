namespace DevRating.Domain.Fake
{
    public sealed class FakeNotContainsRatingOperation : ContainsRatingOperation
    {
        public bool ContainsRatingOf(Entity author)
        {
            return false;
        }

        public bool Contains(object id)
        {
            return false;
        }
    }
}