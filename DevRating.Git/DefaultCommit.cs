namespace DevRating.Git
{
    public class DefaultCommit : Commit
    {
        private readonly string _sha;
        private readonly string _repository;

        public DefaultCommit(string sha, string repository)
        {
            _sha = sha;
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

        public override string ToString()
        {
            return Sha();
        }
    }
}