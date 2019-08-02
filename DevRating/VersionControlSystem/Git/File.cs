using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class File
    {
        private readonly IList<string> _authors;
        private readonly bool _binary;
        private readonly IList<LinesBlock> _deletions;
        private readonly IList<LinesBlock> _additions;
        
        public File(IList<string> authors, bool binary)
        {
            _authors = authors;
            _binary = binary;
            _deletions = new List<LinesBlock>();
            _additions = new List<LinesBlock>();
        }

        public void AddDeletion(LinesBlock deletion)
        {
            _deletions.Add(deletion);
        }
        
        public void AddAddition(LinesBlock addition)
        {
            _additions.Add(addition);
        }

        public bool Binary()
        {
            return _binary;
        }

        public IList<string> Authors()
        {
            if (_binary)
            {
                return new string[0];
            }
            
            IList<string> authors = new List<string>(_authors);

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
            if (_binary)
            {
                return rating;
            }
            
            foreach (var deletion in AscendingDeletions())
            {
                rating = deletion.UpdateRating(rating, _authors);
            }

            return rating;
        }

        private IEnumerable<LinesBlock> AscendingDeletions()
        {
            var deletions = _deletions.ToList();

            deletions.Sort();

            return deletions;
        }

        private IEnumerable<LinesBlock> DescendingDeletions()
        {
            return AscendingDeletions().Reverse();
        }

        private IEnumerable<LinesBlock> AscendingAdditions()
        {
            var additions = _additions.ToList();

            additions.Sort();

            return additions;
        }
    }
}