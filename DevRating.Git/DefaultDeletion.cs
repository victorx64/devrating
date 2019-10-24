namespace DevRating.Git
{
    public sealed class DefaultDeletion : Deletion
    {
        private readonly Commit _commit;
        private readonly Commit _previous;
        private readonly int _count;

        public DefaultDeletion(Commit commit, Commit previous, int count)
        {
            _commit = commit;
            _previous = previous;
            _count = count;
        }

        public Commit Commit()
        {
            return _commit;
        }

        public Commit PreviousCommit()
        {
            return _previous;
        }

        public int Count()
        {
            return _count;
        }
    }
}