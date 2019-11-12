using DevRating.Domain;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class DefaultDeletion : Deletion
    {
        private readonly Commit _commit;
        private readonly Commit _previous;
        private readonly uint _count;

        public DefaultDeletion(Commit commit, Commit previous, uint count)
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

        public uint Count()
        {
            return _count;
        }

        public Deletion NewDeletion(uint count)
        {
            if (count.Equals(_count))
            {
                return this;
            }

            return new DefaultDeletion(_commit, _previous, count);
        }
    }
}