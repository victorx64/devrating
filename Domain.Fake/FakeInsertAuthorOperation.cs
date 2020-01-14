namespace DevRating.Domain.Fake
{
    public sealed class FakeInsertAuthorOperation : InsertAuthorOperation
    {
        private readonly Author _author;

        public FakeInsertAuthorOperation(Author author)
        {
            _author = author;
        }

        public Author Insert(string email)
        {
            return _author;
        }
    }
}