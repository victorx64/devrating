using System.Collections.Generic;
using System.Linq;

namespace DevRating.Git
{
    public sealed class TextFile : File
    {
        private readonly IList<string> _authors;
        private readonly IList<DefaultHunk> _deletions;
        private readonly IList<DefaultHunk> _additions;

        public TextFile() : this(new List<string>(), new List<DefaultHunk>(), new List<DefaultHunk>())
        {
        }

        public TextFile(IList<string> authors, IList<DefaultHunk> deletions, IList<DefaultHunk> additions)
        {
            _authors = authors;
            _deletions = deletions;
            _additions = additions;
        }

        public File PatchedFile(bool binary, string author, string patch)
        {
            if (binary)
            {
                return new BinaryFile();
            }

            var deletions = new List<DefaultHunk>();
            var additions = new List<DefaultHunk>();

            var lines = patch.Split('\n');

            foreach (var line in lines)
            {
                if (!line.StartsWith("@@ "))
                {
                    continue;
                }

                var parts = line.Split(' ');

                deletions.Add(new DefaultHunk(author, parts[1]));
                additions.Add(new DefaultHunk(author, parts[2]));
            }

            return new TextFile(Authors(), deletions, additions);
        }

        public IEnumerable<DefaultAuthorChange> ChangedAuthors()
        {
            var changes = new List<DefaultAuthorChange>();

            foreach (var deletion in AscendingDeletions())
            {
                changes.AddRange(deletion.ChangedAuthors(_authors));
            }

            return changes;
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

        private IEnumerable<DefaultHunk> AscendingDeletions()
        {
            var deletions = _deletions.ToList();

            deletions.Sort();

            return deletions;
        }

        private IEnumerable<DefaultHunk> AscendingAdditions()
        {
            var additions = _additions.ToList();

            additions.Sort();

            return additions;
        }
    }
}