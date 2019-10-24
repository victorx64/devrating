namespace DevRating.Git
{
    public sealed class DefaultCommit : Commit
    {
        private readonly string _sha;
        private readonly Author _author;
        private readonly string _repository;

        public DefaultCommit(string sha, Author author, string repository)
        {
            _sha = sha;
            _author = author;
            _repository = repository;
        }
        
        public string Sha()
        {
            return _sha;
        }

        public string Repository()
        {
            return _repository;
        }

        public Author Author()
        {
            return _author;
        }

        public override string ToString()
        {
            return Sha();
        }
    }
}