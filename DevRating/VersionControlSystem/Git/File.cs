using System.Collections.Generic;
using System.Linq;

namespace DevRating.VersionControlSystem.Git
{
    public class File
    {
        private readonly IEnumerable<string> _authors;
        private readonly bool _binary;
        private readonly IList<LinesBlock> _deletions;
        private readonly IList<LinesBlock> _additions;
        
        public File(IEnumerable<string> authors, bool binary)
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

        public IEnumerable<string> Authors()
        {
            if (_binary)
            {
                return new string[0];
            }
            
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