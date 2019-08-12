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
        private readonly string _oldest;
        private readonly string _newest;
        private readonly Process _process;

        public Git(Player initial, string oldest, string newest) : this(initial, oldest, newest, new DefaultProcess())
        {
        }

        public Git(Player initial, string oldest, string newest, Process process)
        {
            _initial = initial;
            _oldest = oldest;
            _newest = newest;
            _process = process;
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

            return Authors(await Task.WhenAll(BlameTasks(filter, new CompareOptions {ContextLines = 0})));
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

        private IEnumerable<Task<IEnumerable<AuthorChange>>> BlameTasks(CommitFilter filter, CompareOptions options)
        {
            var tasks = new List<Task<IEnumerable<AuthorChange>>>();

            using (var repo = new Repository("."))
            {
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
                                    AuthorChanges(difference.OldPath, difference.Patch, parent.Sha, author)));
                            }
                        }
                    }
                }
            }

            return tasks;
        }

        private IEnumerable<AuthorChange> AuthorChanges(string path, string patch, string commit, string author)
        {
            var changes = new List<AuthorChange>();

            var blames = _process
                .Output("git", $"blame -t -e {commit} -- \"{path}\"")
                .Split('\n');

            foreach (var line in patch.Split('\n'))
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
                        var loser = LineAuthor(blames[i]);

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

        private string LineAuthor(string line)
        {
            var start = line.IndexOf('(');
            var end = line.IndexOf(')');

            return line
                .Substring(start + 1, end - start - 1)
                .Split(' ')[0]
                .TrimStart('<')
                .TrimEnd('>');
        }
    }
}