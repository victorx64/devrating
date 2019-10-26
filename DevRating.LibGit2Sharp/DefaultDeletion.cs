using DevRating.Vcs;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultDeletion : Deletion
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

        public Deletion UpdatedDeletion(int count)
        {
            return new DefaultDeletion(_commit, _previous, count);
        }
    }
}