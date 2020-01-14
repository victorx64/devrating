namespace DevRating.Domain.Fake
{
    public sealed class FakeEntities : Entities
    {
        private readonly Works _works;
        private readonly Ratings _ratings;
        private readonly Authors _authors;

        public FakeEntities(Works works, Ratings ratings, Authors authors)
        {
            _works = works;
            _ratings = ratings;
            _authors = authors;
        }
        
        public Works Works()
        {
            return _works;
        }

        public Ratings Ratings()
        {
            return _ratings;
        }

        public Authors Authors()
        {
            return _authors;
        }
    }
}