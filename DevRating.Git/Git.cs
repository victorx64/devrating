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

        public async Task<Repository> Repository()
        {
            var filter = CommitFilter();

            using (var repo = new LibGit2Sharp.Repository(_path))
            {
                return new Repository(
                    await Task.WhenAll(PatchTasks(repo, filter, new CompareOptions {ContextLines = 0})));
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

        private IEnumerable<Task<Patch>> PatchTasks(IRepository repo, CommitFilter filter, CompareOptions options)
        {
            var tasks = new List<Task<Patch>>();

            foreach (var commit in repo.Commits.QueryBy(filter))
            {
                tasks.AddRange(CommitPatchTasks(repo, options, commit));
            }

            return tasks;
        }

        private IEnumerable<Task<Patch>> CommitPatchTasks(IRepository repo, CompareOptions options, Commit commit)
        {
            var tasks = new List<Task<Patch>>();

            var author = new Author(Email(repo, commit.Author));

            foreach (var parent in commit.Parents)
            {
                tasks.AddRange(ParentCommitPatchTasks(repo, options, commit, parent, author));
            }

            return tasks;
        }

        private IEnumerable<Task<Patch>> ParentCommitPatchTasks(IRepository repo, CompareOptions options, Commit commit,
            Commit parent,
            Author author)
        {
            var tasks = new List<Task<Patch>>();

            var patches = repo.Diff.Compare<LibGit2Sharp.Patch>(parent.Tree, commit.Tree, options);

            foreach (var patch in patches)
            {
                if (!patch.IsBinaryComparison &&
                    patch.OldMode == Mode.NonExecutableFile &&
                    patch.Mode == Mode.NonExecutableFile &&
                    (patch.Status == ChangeKind.Deleted ||
                     patch.Status == ChangeKind.Modified))
                {
                    tasks.Add(Task.Run(() => Patch(repo, patch, commit, parent, author)));
                }
            }

            return tasks;
        }

        private Patch Patch(IRepository repo, PatchEntryChanges patch, GitObject commit,
            Commit parent, Author author)
        {
            var additions = 0;
            var deletions = new List<Author>();

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

                    deletions.AddRange(Deletions(repo, parts[1], blame));
                    additions += Additions(parts[2]);
                }
            }

            return new Patch(author, deletions, additions, commit.Sha);
        }

        private IEnumerable<Author> Deletions(IRepository repo, string hunk, BlameHunkCollection blame)
        {
            var deletions = new List<Author>();

            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            for (var i = index; i < index + count; i++)
            {
                deletions.Add(new Author(Email(repo, blame.HunkForLine(i).FinalSignature)));
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