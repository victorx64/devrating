using DevRating.Domain;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeHunk : Hunk
    {
        private readonly Deletions _deletions;
        private readonly Additions _additions;

        public FakeHunk(Deletions deletions, Additions additions)
        {
            _deletions = deletions;
            _additions = additions;
        }

        public Deletions Deletions()
        {
            return _deletions;
        }

        public Additions Additions()
        {
            return _additions;
        }
    }
}