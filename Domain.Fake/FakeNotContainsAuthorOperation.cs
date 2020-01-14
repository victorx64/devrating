namespace DevRating.Domain.Fake
{
    public sealed class FakeNotContainsAuthorOperation : ContainsAuthorOperation
    {
        public bool Contains(string email)
        {
            return false;
        }

        public bool Contains(object id)
        {
            return false;
        }
    }
}