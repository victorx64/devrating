using DevRating.Vcs;

namespace DevRating.LibGit2Sharp
{
    internal sealed class DefaultAddition : Addition
    {
        private readonly Commit _commit;
        private readonly int _count;

        public DefaultAddition(Commit commit, int count)
        {
            _commit = commit;
            _count = count;
        }

        public Commit Commit()
        {
            return _commit;
        }

        public int Count()
        {
            return _count;
        }

        public Addition NewAddition(int count)
        {
            return new DefaultAddition(_commit, count);
        }
    }
}