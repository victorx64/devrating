namespace DevRating.Domain
{
    public sealed class DefaultInsertWorkParams : InsertWorkParams
    {
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly uint _additions;

        public DefaultInsertWorkParams() : this(string.Empty, string.Empty, string.Empty, 1)
        {
        }

        public DefaultInsertWorkParams(string repository, string start, string end, uint additions)
        {
            _repository = repository;
            _start = start;
            _end = end;
            _additions = additions;
        }

        public Work InsertUsing(InsertWorkOperation operation, Entity author)
        {
            return operation.Insert(_repository, _start, _end, author, _additions);
        }

        public Work InsertUsing(InsertWorkOperation operation, Entity author, Entity rating)
        {
            return operation.Insert(_repository, _start, _end, author, _additions, rating);
        }
    }
}