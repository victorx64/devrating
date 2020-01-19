namespace DevRating.Domain
{
    public sealed class DefaultWorkFactory : WorkFactory
    {
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly uint _additions;

        public DefaultWorkFactory(string repository, string start, string end, uint additions)
        {
            _repository = repository;
            _start = start;
            _end = end;
            _additions = additions;
        }

        public Work WorkFrom(Works works)
        {
            return works.GetOperation().Work(_repository, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_repository, _start, _end);
        }

        public Work InsertedWork(Entities entities, Entity author)
        {
            return entities.Works().InsertOperation().Insert(_repository, _start, _end, author, _additions,
                entities.Ratings().GetOperation().RatingOf(author), new NullDbParameter());
        }
    }
}