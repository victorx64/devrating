using System;
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
            if (_count < 0)
            {
                throw new Exception("");
            }

            return _commit;
        }

        public Commit PreviousCommit()
        {
            if (_count < 0)
            {
                throw new Exception("");
            }

            return _previous;
        }

        public int Count()
        {
            return _count;
        }

        public Deletion NewDeletion(int count)
        {
            if (count.Equals(_count))
            {
                return this;
            }

            return new DefaultDeletion(_commit, _previous, count);
        }
    }
}