using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class File
    {
        private readonly IList<string> _authors;
        private readonly bool _binary;
        private readonly IList<IDeletionHunk> _deletions;
        private readonly IList<IAdditionHunk> _additions;

        public File(IList<string> authors, bool binary) : this(authors, binary, new List<IDeletionHunk>(),
            new List<IAdditionHunk>())
        {
        }

        public File(IList<string> authors, bool binary, IList<IDeletionHunk> deletions, IList<IAdditionHunk> additions)
        {
            _authors = authors;
            _binary = binary;
            _deletions = deletions;
            _additions = additions;
        }

        public void AddDeletion(IDeletionHunk deletion)
        {
            _deletions.Add(deletion);
        }

        public void AddAddition(IAdditionHunk addition)
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

        public IPlayers UpdatedPlayers(IPlayers players)
        {
            if (_binary)
            {
                return players;
            }

            foreach (var deletion in AscendingDeletions())
            {
                players = deletion.UpdatedPlayers(players, _authors);
            }

            return players;
        }

        private IEnumerable<IDeletionHunk> AscendingDeletions()
        {
            var deletions = _deletions.ToList();

            deletions.Sort();

            return deletions;
        }

        private IEnumerable<IDeletionHunk> DescendingDeletions()
        {
            return AscendingDeletions().Reverse();
        }

        private IEnumerable<IAdditionHunk> AscendingAdditions()
        {
            var additions = _additions.ToList();

            additions.Sort();

            return additions;
        }
    }
}