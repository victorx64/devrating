using System;
using System.Collections.Generic;
using DevRating.Git;
using LibGit2Sharp;

namespace DevRating.LibGit2Sharp
{
    internal sealed class FilePatch
    {
        private readonly string _patch;
        private readonly BlameHunkCollection _blame;
        private readonly IRepository _repository;

        public FilePatch(string patch, BlameHunkCollection blame, IRepository repository)
        {
            _patch = patch;
            _blame = blame;
            _repository = repository;
        }

        public void WriteInto(Modifications modifications)
        {
            foreach (var line in _patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    // TODO Throw exception on contextual lines. Patch must be without contextual lines (git log -U0)
                    var parts = line.Split(' ');

                    foreach (var deletion in Deletions(parts[1]))
                    {
                        modifications.AddDeletion(deletion);
                    }

                    modifications.AddAdditions(Additions(parts[2]));
                }
            }
        }

        private IEnumerable<string> Deletions(string hunk)
        {
            var deletions = new List<string>();

            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            for (var i = index; i < index + count; i++)
            {
                deletions.Add(_repository.Mailmap.ResolveSignature(_blame.HunkForLine(i).FinalSignature).Email);
            }

            return deletions;
        }

        private int Additions(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            return count;
        }
    }
}