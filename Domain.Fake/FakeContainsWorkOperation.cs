namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsWorkOperation : ContainsWorkOperation
    {
        public bool Contains(string repository, string start, string end)
        {
            return true;
        }

        public bool Contains(object id)
        {
            return true;
        }
    }
}