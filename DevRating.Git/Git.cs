using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git
    {
        private readonly string _path;
        private readonly string _oldest;
        private readonly string _newest;

        public Git(string path, string oldest, string newest)
        {
            _path = path;
            _oldest = oldest;
            _newest = newest;
        }

        public async Task LogAuthorChanges(AuthorsLog log)
        {
            foreach (var hunks in await Patches())
            {
                foreach (var hunk in hunks)
                {
                    hunk.LogAuthorChanges(log);
                }
            }
        }

        private async Task<IEnumerable<IEnumerable<Hunk>>> Patches()
        {
            var filter = CommitFilter();

            var options = new CompareOptions
            {
                ContextLines = 0
            };

            using (var repo = new Repository(_path))
            {
                var tasks = HunkTasks(repo, filter, options);

                return await Task.WhenAll(tasks);
            }
        }

        private CommitFilter CommitFilter()
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

            return filter;
        }

        private IEnumerable<Task<IEnumerable<Hunk>>> HunkTasks(IRepository repo, CommitFilter filter,
            CompareOptions options)
        {
            var tasks = new List<Task<IEnumerable<Hunk>>>();

            foreach (var commit in repo.Commits.QueryBy(filter))
            {
                tasks.AddRange(CommitHunkTasks(repo, options, commit));
            }

            return tasks;
        }

        private IEnumerable<Task<IEnumerable<Hunk>>> CommitHunkTasks(IRepository repo, CompareOptions options,
            Commit commit)
        {
            var tasks = new List<Task<IEnumerable<Hunk>>>();

            var author = Email(repo, commit.Author);

            foreach (var parent in commit.Parents)
            {
                tasks.AddRange(ParentCommitHunkTasks(repo, options, commit, parent, author));
            }

            return tasks;
        }

        private IEnumerable<Task<IEnumerable<Hunk>>> ParentCommitHunkTasks(IRepository repo, CompareOptions options,
            Commit commit,
            Commit parent,
            string author)
        {
            var tasks = new List<Task<IEnumerable<Hunk>>>();

            var patches = repo.Diff.Compare<Patch>(parent.Tree, commit.Tree, options);

            foreach (var patch in patches)
            {
                if (!patch.IsBinaryComparison &&
                    patch.OldMode == Mode.NonExecutableFile &&
                    patch.Mode == Mode.NonExecutableFile &&
                    (patch.Status == ChangeKind.Deleted ||
                     patch.Status == ChangeKind.Modified))
                {
                    tasks.Add(Task.Run(() => Hunks(repo, patch, commit, parent, author)));
                }
            }

            return tasks;
        }

        private IEnumerable<Hunk> Hunks(IRepository repo, PatchEntryChanges patch, GitObject commit,
            Commit parent, string author)
        {
            var hunks = new List<Hunk>();

            var blame = repo.Blame(patch.OldPath, new BlameOptions
            {
                StartingAt = parent
            });

            foreach (var line in patch.Patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    var parts = line.Split(' ');

                    hunks.Add(new Hunk(author, Deletions(repo, parts[1], blame), Additions(parts[2]), commit.Sha));
                }
            }

            return hunks;
        }

        private IEnumerable<string> Deletions(IRepository repo, string hunk, BlameHunkCollection blame)
        {
            var deletions = new List<string>();

            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            for (var i = index; i < index + count; i++)
            {
                deletions.Add(Email(repo, blame.HunkForLine(i).FinalSignature));
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

        private string Email(IRepository repo, Signature signature)
        {
            return string.IsNullOrEmpty(signature.Name) ||
                   string.IsNullOrEmpty(signature.Email)
                ? signature.Email
                : repo.Mailmap.ResolveSignature(signature).Email;
        }
    }
}