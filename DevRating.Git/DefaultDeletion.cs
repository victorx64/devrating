namespace DevRating.Git
{
    public class DefaultDeletion : Deletion
    {
        private readonly Author _author;
        private readonly Commit _commit;
        private readonly Author _victim;
        private readonly int _count;

        public DefaultDeletion(Author author, Commit commit, Author victim, int count)
        {
            _author = author;
            _commit = commit;
            _victim = victim;
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

        public Author Victim()
        {
            return _victim;
        }

        public int Count()
        {
            return _count;
        }
    }
}