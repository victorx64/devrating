namespace DevRating.Git
{
    public class DefaultAddition : Addition
    {
        private readonly Author _author;
        private readonly Commit _commit;
        private readonly int _count;

        public DefaultAddition(Author author, Commit commit, int count)
        {
            _author = author;
            _commit = commit;
            _count = count;
        }
        
        public Author Author()
        {
            return _author;
        }

        public Commit Commit()
        {
            return _commit;
        }

        public int Count()
        {
            return _count;
        }
    }
}