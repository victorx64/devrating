namespace DevRating.Domain
{
    public sealed class LinkedInsertWorkParams : InsertWorkParams
    {
        private readonly string _link;
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly uint _additions;

        public LinkedInsertWorkParams(string link)
            : this(link, string.Empty, string.Empty, string.Empty, 1)
        {
        }

        public LinkedInsertWorkParams(string link, string repository, string start, string end, uint additions)
        {
            _link = link;
            _repository = repository;
            _start = start;
            _end = end;
            _additions = additions;
        }

        public Work InsertionResult(InsertWorkOperation operation, Entity author)
        {
            return operation.Insert(_repository, _start, _end, author, _additions, _link);
        }

        public Work InsertionResult(InsertWorkOperation operation, Entity author, Entity rating)
        {
            return operation.Insert(_repository, _start, _end, author, _additions, rating, _link);
        }
    }
}