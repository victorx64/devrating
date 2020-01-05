using System.Linq;

namespace DevRating.Domain
{
    public sealed class TotalAdditions : Additions
    {
        private readonly Hunks _hunks;

        public TotalAdditions(Hunks hunks)
        {
            _hunks = hunks;
        }

        public uint Count()
        {
            return (uint) _hunks.Items()
                .Sum(HunkAdditions);
        }

        private long HunkAdditions(Hunk hunk)
        {
            return hunk.Additions().Count();
        }
    }
}