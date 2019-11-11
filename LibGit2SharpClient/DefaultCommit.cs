using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Domain.Git;
using LibGit2Sharp;
using Commit = DevRating.Domain.Git.Commit;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class DefaultCommit : Commit
    {
        private readonly string _sha;
        private readonly string _author;
        private readonly IRepository _repository;

        public DefaultCommit(string sha, string author, IRepository repository)
        {
            _sha = sha;
            _author = author;
            _repository = repository;
        }

        public string Sha()
        {
            return _sha;
        }

        public string RepositoryFirstUrl()
        {
            return _repository.Network.Remotes.First().Url;
        }

        public string Author()
        {
            return _author;
        }

        public async Task WriteInto(ModificationsCollection modifications)
        {
            await Task.WhenAll(WriteCommitTasks(modifications, _repository.Lookup<global::LibGit2Sharp.Commit>(_sha)));
        }

        private IEnumerable<Task> WriteCommitTasks(ModificationsCollection modifications,
            global::LibGit2Sharp.Commit commit)
        {
            foreach (var parent in commit.Parents)
            {
                foreach (var task in WriteDifferencesTasks(modifications, commit, parent))
                {
                    yield return task;
                }
            }
        }

        private IEnumerable<Task> WriteDifferencesTasks(
            ModificationsCollection modifications,
            global::LibGit2Sharp.Commit commit,
            global::LibGit2Sharp.Commit parent)
        {
            var differences = _repository.Diff.Compare<Patch>(
                parent.Tree,
                commit.Tree,
                new CompareOptions {ContextLines = 0});

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
                        var blames = _repository.Blame(difference.OldPath, options);

                        WritePatchInto(modifications, difference.Patch, blames, commit);
                    });
                }
            }
        }

        private void WritePatchInto(
            ModificationsCollection modifications,
            string patch,
            BlameHunkCollection blames,
            global::LibGit2Sharp.Commit commit)
        {
            var author = _repository.Mailmap.ResolveSignature(commit.Author).Email;

            var current = new DefaultCommit(commit.Sha, author, _repository);

            foreach (var line in patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    // line must be like "@@ -3,9 +3,9 @@ blah..."
                    // TODO Throw exception on a contextual line. Patch must be without contextual lines (git log -U0)
                    var parts = line.Split(' ');

                    WriteDeletionInto(modifications, current, parts[1], blames);
                    WriteAdditionInto(modifications, current, parts[2]);
                }
            }
        }

        private void WriteDeletionInto(
            ModificationsCollection modifications,
            Commit current,
            string hunk,
            BlameHunkCollection blames)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToUInt32(parts[0]) - 1;

            var count = parts.Length == 1 ? 1 : Convert.ToUInt32(parts[1]);

            uint d;

            for (var i = index; i < index + count; i += d)
            {
                var blame = blames.HunkForLine((int) i);
                d = Math.Min((uint) (blame.FinalStartLineNumber + blame.LineCount), index + count) - i;

                var author = _repository.Mailmap.ResolveSignature(blame.FinalSignature).Email;

                var previous = new DefaultCommit(
                    blame.FinalCommit.Sha,
                    author,
                    _repository);

                modifications.AddDeletion(new DefaultDeletion(current, previous, d));
            }
        }

        private void WriteAdditionInto(ModificationsCollection modifications, Commit current, string hunk)
        {
            modifications.AddAddition(new DefaultAddition(current, AdditionsCount(hunk)));
        }

        private uint AdditionsCount(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var count = parts.Length == 1 ? 1 : Convert.ToUInt32(parts[1]);

            return count;
        }

        public override string ToString()
        {
            return Sha();
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is DefaultCommit other && Equals(other);
        }

        private bool Equals(DefaultCommit other)
        {
            return string.Equals(_sha, other._sha, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(_author, other._author, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(RepositoryFirstUrl(), other.RepositoryFirstUrl(), StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(_sha);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_author);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(RepositoryFirstUrl());
                return hashCode;
            }
        }
    }
}