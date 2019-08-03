using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class File : IFile
    {
        private readonly IList<string> _authors;
        private readonly IList<IDeletionHunk> _deletions;
        private readonly IList<IAdditionHunk> _additions;

        public File() : this(new List<string>(), new List<IDeletionHunk>(), new List<IAdditionHunk>())
        {
        }

        public File(IList<string> authors) : this(authors, new List<IDeletionHunk>(), new List<IAdditionHunk>())
        {
        }

        public File(IList<string> authors, IList<IDeletionHunk> deletions, IList<IAdditionHunk> additions)
        {
            _authors = authors;
            _deletions = deletions;
            _additions = additions;
        }

        public IFile SolidifiedFile(bool binary)
        {
            if (binary)
            {
                return new BinaryFile();
            }

            return new File(Authors());
        }
        
        public void ApplyPatch(string author, string patch)
        {
            var lines = patch.Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    _deletions.Add(new DeletionHunk(author, parts[1]));
                    _additions.Add(new AdditionHunk(author, parts[2]));
                }
            }
        }

        public IPlayers UpdatedPlayers(IPlayers players)
        {
            foreach (var deletion in AscendingDeletions())
            {
                players = deletion.UpdatedPlayers(players, _authors);
            }

            return players;
        }

        private IList<string> Authors()
        {
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