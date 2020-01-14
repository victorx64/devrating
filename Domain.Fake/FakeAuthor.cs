namespace DevRating.Domain.Fake
{
    public sealed class FakeAuthor : Author
    {
        public object Id()
        {
            throw new System.NotImplementedException();
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public string Email()
        {
            throw new System.NotImplementedException();
        }

        public Rating Rating()
        {
            throw new System.NotImplementedException();
        }

        public bool HasRating()
        {
            throw new System.NotImplementedException();
        }
    }
}