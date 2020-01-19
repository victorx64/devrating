namespace DevRating.Domain
{
    public sealed class DefaultDiffFingerprint : DiffFingerprint
    {
        private readonly AuthorFactory _authors;
        private readonly WorkFactory _works;
        private readonly RatingFactory _ratings;

        public DefaultDiffFingerprint(string repository, string start, string end, uint additions, string email,
            Deletions deletions)
            : this(repository, start, end, additions, email, new DefaultAuthorFactory(email), deletions)
        {
        }

        public DefaultDiffFingerprint(string repository, string start, string end, uint additions, string email,
            AuthorFactory authors, Deletions deletions)
            : this(authors, new DefaultWorkFactory(repository, start, end, additions),
                new DefaultRatingFactory(deletions, email, authors))
        {
        }

        public DefaultDiffFingerprint(AuthorFactory authors, WorkFactory works, RatingFactory ratings)
        {
            _authors = authors;
            _works = works;
            _ratings = ratings;
        }

        public Work WorkFrom(Works works)
        {
            return _works.WorkFrom(works);
        }

        public bool PresentIn(Works works)
        {
            return _works.PresentIn(works);
        }

        public void AddTo(Entities entities, Formula formula)
        {
            var author = _authors.Author(entities);

            _ratings.InsertNewRatings(entities, formula, author, _works.InsertedWork(entities, author));
        }
    }
}