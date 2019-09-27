using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class FilePatch : Watchdog
    {
        private readonly string _patch;
        private readonly Blame _blame;

        public FilePatch(string patch, Blame blame)
        {
            _patch = patch;
            _blame = blame;
        }

        public void WriteInto(Modifications modifications)
        {
            foreach (var line in _patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    var parts = line.Split(' ');

                    new Hunk(Deletions(parts[1], _blame), Additions(parts[2]))
                        .WriteInto(modifications);
                }
            }
        }

        private IEnumerable<string> Deletions(string hunk, Blame blame)
        {
            var deletions = new List<string>();

            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            for (var i = index; i < index + count; i++)
            {
                deletions.Add(blame.AuthorOf(i));
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