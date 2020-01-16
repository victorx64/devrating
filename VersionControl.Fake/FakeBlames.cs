using System.Collections.Generic;
using System.Linq;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeBlames : Blames
    {
        private readonly IEnumerable<Blame> _blames;

        public FakeBlames(IEnumerable<Blame> blames)
        {
            _blames = blames;
        }

        public Blame HunkForLine(uint line)
        {
            bool ContainsLine(Blame x)
            {
                return x.ContainsLine(line);
            }

            return _blames.Single(ContainsLine);
        }
    }
}