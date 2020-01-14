namespace DevRating.Domain.Fake
{
    public sealed class FakeGetRatingOperation : GetRatingOperation
    {
        private readonly Rating _rating;

        public FakeGetRatingOperation(Rating rating)
        {
            _rating = rating;
        }

        public Rating RatingOf(Entity author)
        {
            return _rating;
        }

        public Rating Rating(object id)
        {
            return _rating;
        }
    }
}