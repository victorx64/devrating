namespace DevRating.Domain.Fake
{
    public sealed class FakeInsertWorkOperation : InsertWorkOperation
    {
        private readonly Work _work;

        public FakeInsertWorkOperation(Work work)
        {
            _work = work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating)
        {
            return _work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions)
        {
            return _work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating,
            string link)
        {
            return _work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, string link)
        {
            return _work;
        }
    }
}