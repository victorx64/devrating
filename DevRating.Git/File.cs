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

        public File(IList<string> authors, IList<IDeletionHunk> deletions, IList<IAdditionHunk> additions)
        {
            _authors = authors;
            _deletions = deletions;
            _additions = additions;
        }

        public IFile PatchedFile(bool binary, string author, string patch)
        {
            if (binary)
            {
                return new BinaryFile();
            }

            var deletions = new List<IDeletionHunk>();
            var additions = new List<IAdditionHunk>();
            
            var lines = patch.Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ');

                    deletions.Add(new DeletionHunk(author, parts[1]));
                    additions.Add(new AdditionHunk(author, parts[2]));
                }
            }
            
            return new File(Authors(), deletions, additions);
        }

        public IPlayers UpdatedDevelopers(IPlayers developers)
        {
            foreach (var deletion in AscendingDeletions())
            {
                developers = deletion.UpdatedPlayers(developers, _authors);
            }

            return developers;
        }

        private IList<string> Authors()
        {
            IList<string> authors = new List<string>(_authors);

            foreach (var deletion in AscendingDeletions().Reverse())
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

        private IEnumerable<IAdditionHunk> AscendingAdditions()
        {
            var additions = _additions.ToList();

            additions.Sort();

            return additions;
        }
    }
}