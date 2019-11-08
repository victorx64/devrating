using DevRating.Domain.Git;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultAddition : Addition
    {
        private readonly Commit _commit;
        private readonly uint _count;

        public DefaultAddition(Commit commit, uint count)
        {
            _commit = commit;
            _count = count;
        }

        public Commit Commit()
        {
            return _commit;
        }

        public uint Count()
        {
            return _count;
        }

        public Addition NewAddition(uint count)
        {
            if (count.Equals(_count))
            {
                return this;
            }

            return new DefaultAddition(_commit, count);
        }
    }
}