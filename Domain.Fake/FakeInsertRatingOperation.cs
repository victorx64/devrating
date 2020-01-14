namespace DevRating.Domain.Fake
{
    public sealed class FakeInsertRatingOperation : InsertRatingOperation
    {
        private readonly Rating _rating;

        public FakeInsertRatingOperation(Rating rating)
        {
            _rating = rating;
        }

        public Rating Insert(Entity author, double value, Entity work)
        {
            return _rating;
        }

        public Rating Insert(Entity author, double value, Entity previous, Entity work)
        {
            return _rating;
        }
    }
}