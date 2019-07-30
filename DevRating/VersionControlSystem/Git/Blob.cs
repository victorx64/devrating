using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class Blob
    {
        private readonly IEnumerable<string> _authors;
        private readonly IEnumerable<LinesBlock> _deletions;
        private readonly IEnumerable<LinesBlock> _additions;

        public Blob() : this(new string[0],new LinesBlock[0], new LinesBlock[0])
        {
        }

        public Blob(IEnumerable<string> authors, IEnumerable<LinesBlock> deletions,
            IEnumerable<LinesBlock> additions)
        {
            _authors = authors;
            _deletions = deletions;
            _additions = additions;
        }

        public IEnumerable<string> Authors()
        {
            IEnumerable<string> authors = new List<string>(_authors);

            foreach (var deletion in DescendingDeletions())
            {
                authors = deletion.DeleteFrom(authors);
            }

            foreach (var addition in AscendingAdditions())
            {
                authors = addition.AddTo(authors);
            }

            return authors;
        }

        public IRating UpdateRating(IRating rating)
        {
            foreach (var deletion in _deletions)
            {
                rating = deletion.UpdateRating(rating, _authors);
            }
            
            return rating;
        }

        private IEnumerable<LinesBlock> DescendingDeletions()
        {
            var deletions = _deletions.ToList();

            deletions.Sort();

            deletions.Reverse();

            return deletions;
        }

        private IEnumerable<LinesBlock> AscendingAdditions()
        {
            var additions = _additions.ToList();

            additions.Sort();

            return additions;
        }
    }
}