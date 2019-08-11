using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : AuthorsCollection
    {
        private readonly IDictionary<string, Player> _authors;
        private readonly Player _initial;
        private readonly string _oldest;
        private readonly string _latest;
        private readonly Process _process = new DefaultProcess();

        public Git(IDictionary<string, Player> authors, Player initial, string oldest, string latest)
        {
            _authors = authors;
            _initial = initial;
            _oldest = oldest;
            _latest = latest;
        }

        public async Task<IDictionary<string, Player>> Authors()
        {
            var authors = _authors;

            var compareOptions = new CompareOptions
            {
                ContextLines = 0
            };

            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological |
                         CommitSortStrategies.Reverse
            };

            if (!string.IsNullOrEmpty(_oldest))
            {
                filter.ExcludeReachableFrom = new ObjectId(_oldest);
            }

            if (!string.IsNullOrEmpty(_latest))
            {
                filter.IncludeReachableFrom = new ObjectId(_latest);
            }

            var tasks = new List<Task<IEnumerable<AuthorChange>>>();

            using (var repo = new Repository("."))
            {
                foreach (var current in repo.Commits.QueryBy(filter))
                {
                    var author = repo.Mailmap.ResolveSignature(current.Author).Email;

                    foreach (var parent in current.Parents)
                    {
                        var differences = repo.Diff.Compare<Patch>(parent.Tree, current.Tree, compareOptions);

                        foreach (var difference in differences)
                        {
                            if (!difference.IsBinaryComparison &&
                                (difference.Status == ChangeKind.Deleted ||
                                 difference.Status == ChangeKind.Modified))
                            {
                                tasks.Add(Task.Run(() =>
                                    Changes(difference.OldPath, difference.Patch, parent.Sha, author)));
                            }
                        }
                    }
                }

                var collections = await Task.WhenAll(tasks);

                foreach (var changes in collections)
                {
                    foreach (var change in changes)
                    {
                        authors = change.UpdatedAuthors(authors);
                    }
                }
            }

            return authors;
        }

        private IEnumerable<AuthorChange> Changes(string path, string patch, string commit, string author)
        {
            var lines = patch.Split('\n');

            var changes = new List<AuthorChange>();

            var output = _process.Output("git", $"blame -t -e {commit} -- {path}");

            var blames = output.ReadToEnd().Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("@@ "))
                {
                    var parts = line.Split(' ')[1]
                        .Substring(1)
                        .Split(',');

                    var index = Convert.ToInt32(parts[0]) - 1;

                    var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

                    for (var i = index; i < index + count; i++)
                    {
                        var meta = Metadata(blames[i]);

                        var loser = Author(meta);

                        if (loser.Equals(author))
                        {
                            continue;
                        }

                        changes.Add(new DefaultAuthorChange(loser, author, _initial));
                    }
                }
            }

            return changes;
        }
        
        private string[] Metadata(string line)
        {
            var start = line.IndexOf('(');

            var end = line.IndexOf(')');

            var length = end - start - 1;

            var metadata = line.Substring(start + 1, length);
            
            return metadata.Split(' ');
        }

        private string Author(string[] metadata)
        {
            return metadata[0].Trim('<', '>');
        }
    }
}