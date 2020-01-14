namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsAuthorOperation : ContainsAuthorOperation
    {
        public bool Contains(string email)
        {
            return true;
        }

        public bool Contains(object id)
        {
            return true;
        }
    }
}