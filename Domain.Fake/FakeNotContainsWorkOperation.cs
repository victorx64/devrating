namespace DevRating.Domain.Fake
{
    public sealed class FakeNotContainsWorkOperation : ContainsWorkOperation
    {
        public bool Contains(string repository, string start, string end)
        {
            return false;
        }

        public bool Contains(object id)
        {
            return false;
        }
    }
}