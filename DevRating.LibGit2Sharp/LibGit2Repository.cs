using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Git;
using LibGit2Sharp;
using Commit = DevRating.Git.Commit;
using Repository = DevRating.Git.Repository;

namespace DevRating.LibGit2Sharp
{
    public sealed class LibGit2Repository : Repository, IDisposable
    {
        private readonly IRepository _repository;
        private readonly string _id;

        public LibGit2Repository(string path, string id) : this(new global::LibGit2Sharp.Repository(path), id)
        {
        }

        public LibGit2Repository(IRepository repository, string id)
        {
            _repository = repository;
            _id = id;
        }

        public async Task WriteInto(Modifications modifications, string sha)
        {
            var commit = _repository.Lookup<global::LibGit2Sharp.Commit>(sha);

            var options = new CompareOptions
            {
                ContextLines = 0
            };

            var author = new DefaultAuthor(_repository.Mailmap.ResolveSignature(commit.Author).Email);

            await Task.WhenAll(CommitWriteTasks(modifications, commit, options, author));
        }

        private IEnumerable<Task> CommitWriteTasks(Modifications modifications, global::LibGit2Sharp.Commit commit,
            CompareOptions options, Author author)
        {
            foreach (var parent in commit.Parents)
            {
                var differences = _repository.Diff.Compare<Patch>(parent.Tree, commit.Tree, options);

                foreach (var task in DifferencesWriteTasks(modifications, differences, parent.Sha,
                    new DefaultCommit(commit.Sha, _id), author))
                {
                    yield return task;
                }
            }
        }

        private IEnumerable<Task> DifferencesWriteTasks(Modifications modifications, Patch differences, string parent,
            Commit commit, Author author)
        {
            var options = new BlameOptions {StartingAt = parent};

            foreach (var difference in differences)
            {
                if (!difference.IsBinaryComparison &&
                    difference.OldMode == Mode.NonExecutableFile &&
                    difference.Mode == Mode.NonExecutableFile &&
                    (difference.Status == ChangeKind.Deleted ||
                     difference.Status == ChangeKind.Modified))
                {
                    yield return Task.Run(() =>
                    {
                        var blame = _repository.Blame(difference.OldPath, options);

                        WritePatchInto(modifications, difference.Patch, blame, commit, author);
                    });
                }
            }
        }

        private void WritePatchInto(Modifications modifications, string patch, BlameHunkCollection blame, Commit commit,
            Author author)
        {
            foreach (var line in patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    // TODO Throw exception on a contextual line. Patch must be without contextual lines (git log -U0)
                    var parts = line.Split(' ');

                    var deletions = Deletions(parts[1], blame)
                        .GroupBy(s => s);

                    foreach (var deletion in deletions)
                    {
                        modifications
                            .AddDeletion(
                                new DefaultDeletion(
                                    author,
                                    commit,
                                    new DefaultAuthor(deletion.Key),
                                    deletion.Count()));
                    }

                    modifications.AddAddition(new DefaultAddition(author, commit, Additions(parts[2])));
                }
            }
        }

        private IEnumerable<string> Deletions(string hunk, BlameHunkCollection blame)
        {
            var deletions = new List<string>();

            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToInt32(parts[1]);

            for (var i = index; i < index + count; i++)
            {
                deletions.Add(_repository.Mailmap.ResolveSignature(blame.HunkForLine(i).FinalSignature).Email);
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

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}