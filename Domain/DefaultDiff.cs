namespace DevRating.Domain
{
    public sealed class DefaultDiff : Diff
    {
        private readonly string _repository;
        private readonly string _start;
        private readonly string _end;
        private readonly uint _additions;
        private readonly string _email;
        private readonly Deletions _deletions;

        public DefaultDiff(string repository, string start, string end, uint additions, string email,
            Deletions deletions)
        {
            _repository = repository;
            _start = start;
            _end = end;
            _additions = additions;
            _email = email;
            _deletions = deletions;
        }

        public Work From(Works works)
        {
            return works.GetOperation().Work(_repository, _start, _end);
        }

        public bool PresentIn(Works works)
        {
            return works.ContainsOperation().Contains(_repository, _start, _end);
        }

        public void AddTo(EntitiesFactory factory)
        {
            var work = factory.InsertedWork(_repository, _start, _end, _email, _additions);
            
            factory.InsertRatings(_email, _deletions, work);
        }
    }
}