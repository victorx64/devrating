using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : AuthorsCollection
    {
        private readonly Player _initial;
        private readonly string _path;
        private readonly string _oldest;
        private readonly string _newest;

        public Git(Player initial, string path, string oldest, string newest)
        {
            _initial = initial;
            _path = path;
            _oldest = oldest;
            _newest = newest;
        }

        public async Task<IDictionary<string, Player>> Authors()
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological |
                         CommitSortStrategies.Reverse
            };

            if (!string.IsNullOrEmpty(_oldest))
            {
                filter.ExcludeReachableFrom = new ObjectId(_oldest);
            }

            if (!string.IsNullOrEmpty(_newest))
            {
                filter.IncludeReachableFrom = new ObjectId(_newest);
            }

            using (var repo = new Repository(_path))
            {
                return Authors(await Task.WhenAll(BlameTasks(repo, filter, new CompareOptions {ContextLines = 0})));
            }
        }

        private IDictionary<string, Player> Authors(IEnumerable<IEnumerable<AuthorChange>> collections)
        {
            IDictionary<string, Player> authors = new Dictionary<string, Player>();

            foreach (var changes in collections)
            {
                foreach (var change in changes)
                {
                    authors = change.UpdatedAuthors(authors);
                }
            }

            return authors;
        }

        private IEnumerable<Task<IEnumerable<AuthorChange>>> BlameTasks(IRepository repo, CommitFilter filter,
            CompareOptions options)
        {
            var tasks = new List<Task<IEnumerable<AuthorChange>>>();

            foreach (var current in repo.Commits.QueryBy(filter))
            {
                if (current.Parents.Count() == 1)
                {
                    var author = string.IsNullOrEmpty(current.Author.Name) ||
                                 string.IsNullOrEmpty(current.Author.Email)
                        ? current.Author.Email
                        : repo.Mailmap.ResolveSignature(current.Author).Email;

                    var parent = current.Parents.First();

                    var differences = repo.Diff.Compare<Patch>(parent.Tree, current.Tree, options);

                    foreach (var difference in differences)
                    {
                        if (!difference.IsBinaryComparison &&
                            difference.OldMode == Mode.NonExecutableFile &&
                            difference.Mode == Mode.NonExecutableFile &&
                            (difference.Status == ChangeKind.Deleted ||
                             difference.Status == ChangeKind.Modified))
                        {
                            tasks.Add(Task.Run(() =>
                                AuthorChanges(repo, difference, parent, author)));
                        }
                    }
                }
            }

            return tasks;
        }

        private IEnumerable<AuthorChange> AuthorChanges(IRepository repo, PatchEntryChanges difference, Commit commit,
            string author)
        {
            var changes = new List<AuthorChange>();

            var blame = repo.Blame(difference.OldPath, new BlameOptions
            {
                StartingAt = commit
            });

            foreach (var line in difference.Patch.Split('\n'))
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
                        var loser = repo.Mailmap.ResolveSignature(blame.HunkForLine(i).FinalSignature).Email;

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
    }
}