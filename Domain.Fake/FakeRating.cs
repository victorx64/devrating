namespace DevRating.Domain.Fake
{
    public sealed class FakeRating : Rating
    {
        public object Id()
        {
            throw new System.NotImplementedException();
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public double Value()
        {
            throw new System.NotImplementedException();
        }

        public bool HasPreviousRating()
        {
            throw new System.NotImplementedException();
        }

        public Rating PreviousRating()
        {
            throw new System.NotImplementedException();
        }

        public Work Work()
        {
            throw new System.NotImplementedException();
        }

        public Author Author()
        {
            throw new System.NotImplementedException();
        }
    }
}